using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class PageViewModel : ParagraphBaseViewModel<ParagraphModel>, IDisposable
    {
        public PageViewModel()
        {
            SwipeLeftCommand = CommandHelper.Create(SwipeLeft);
            SwipeRightCommand = CommandHelper.Create(SwipeRight);
            TouchedCommand = CommandHelper.Create<ParagraphModel>(Touch);
            LongTouchedCommand = CommandHelper.Create<ParagraphModel>(LongTouched);
            RemoveReadingCommand = CommandHelper.Create(RemoveReading);
            SelectPageCommand = CommandHelper.Create(SelectPage);
            SelectVerseCommand = CommandHelper.Create(SelectVerse);
            PlayCommand = CommandHelper.Create(Play);
            MultipleSelectOrUnSelectCommand = CommandHelper.Create(MultipleSelectOrUnSelect);
            MultipleSelectionMenuCommand = CommandHelper.Create(MultipleSelectionMenu);
            Task.Factory.StartNew(async () =>
            {
                await PositionUpdater();
            });
        }

        public ICommand<ParagraphModel> TouchedCommand { get; set; }
        public ICommand<ParagraphModel> LongTouchedCommand { get; set; }
        public ICommand MultipleSelectOrUnSelectCommand { get; set; }
        public ICommand MultipleSelectionMenuCommand { get; set; }
        public ICommand SwipeLeftCommand { get; set; }
        public ICommand SwipeRightCommand { get; set; }
        public ICommand RemoveReadingCommand { get; set; }
        public ICommand SelectPageCommand { get; set; }
        public ICommand SelectVerseCommand { get; set; }
        public ICommand PlayCommand { get; set; }

        bool _isLoadingPlayStream = false;
        public bool IsLoadingPlayStream
        {
            get => _isLoadingPlayStream;
            set
            {
                _isLoadingPlayStream = value;
                OnPropertyChanged(nameof(IsLoadingPlayStream));
            }
        }

        bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
                ApplicationHelper.Current.KeepScreenOn(value);
            }
        }

        string _CatalogName = "";
        public string CatalogName
        {
            get => _CatalogName;
            set
            {
                _CatalogName = value;
                OnPropertyChanged(nameof(CatalogName));
            }
        }

        const string EmptyReading = "بدون خوانش";
        public string ReadingName
        {
            get
            {
                if (IsOutsideOfBookTab)
                    return null;
                if (ApplicationReadingData.CurrentReadingData == null)
                    return EmptyReading;
                return ApplicationReadingData.CurrentReadingData.Name;
            }
        }

        /// <summary>
        /// برای وقتی که کاربر روی برو به صفحه کلیک میکند و نباید روی خوانش ها تاثیر گزار باشد
        /// </summary>
        public void SetIsOutsideOfBookTab()
        {
            IsOutsideOfBookTab = true;
            OnPropertyChanged(nameof(ReadingName));
        }

        public bool IsOutsideOfBookTab { get; set; }

        public PageContract Page { get; set; }
        public long BookId { get; set; }
        public long CatalogId { get; set; }

        private long _catalogStartPageNumber;
        public long CatalogStartPageNumber
        {
            get => _catalogStartPageNumber;
            set
            {
                _catalogStartPageNumber = value;
                OnPropertyChanged(nameof(CatalogStartPageNumber));
            }
        }

        private int _ScrollToIndex;
        public int ScrollToIndex
        {
            get => _ScrollToIndex;
            set
            {
                _ScrollToIndex = value;
                OnPropertyChanged(nameof(ScrollToIndex));
            }
        }

        private double _AudioDuration;
        private double _PlaybackCurrentPosition;
        public double PlaybackCurrentPosition
        {
            get
            {
                return _PlaybackCurrentPosition;
            }
            set
            {
                if (_PlaybackCurrentPosition == value)
                    return;
                _PlaybackCurrentPosition = value;
                OnPropertyChanged(nameof(PlaybackCurrentPosition));
                if (AudioPlayerBaseHelper.CurrentBase.CanSeek)
                {
                    Seek(value * _AudioDuration);
                }
            }
        }

        public async Task Initialize(long startPageNumber, long bookId, long catalogId)
        {
            CatalogId = catalogId;
            BookId = bookId;
            CatalogStartPageNumber = startPageNumber;
            _ = Task.Factory.StartNew(async () =>
            {
                await LoadData();
            });
        }

        void ScrollToTop()
        {
            ScrollToIndex = 1;
            ScrollToIndex = 0;
        }

        public override async Task FetchData(bool isForce)
        {
            ResetPlayBack();
            ScrollToTop();
            var pages = await FetchPage(isForce, CatalogStartPageNumber, BookId);
            //fetch next
            _ = Task.Factory.StartNew(async () =>
            {
                await FetchPage(isForce, CatalogStartPageNumber + 1, BookId);
            });
            //fetch prevoius
            _ = Task.Factory.StartNew(async () =>
            {
                await FetchPage(isForce, CatalogStartPageNumber - 1, BookId);
            });
            if (pages.IsSuccess)
            {
                bool isEven = false;
                InitialData(pages.Result.SelectMany(x => x.Paragraphs.Select(i => ParagraphModel.Map(i))).Select(v =>
                {
                    v.IsEven = isEven;
                    isEven = !isEven;
                    return v;
                }));
                var bookId = pages.Result.First().BookId;
                var categoryResult = await TranslatorService.GetBookService(isForce).GetCategoryByBookIdAsync(bookId);
                if (categoryResult.IsSuccess)
                {
                    CatalogName = $"{categoryResult.Result.Names.GetPersianValue()} / ";
                }
                var bookResult = await TranslatorService.GetBookService(isForce).GetBookByIdAsync(bookId);
                if (bookResult.IsSuccess)
                {
                    var value = bookResult.Result.Names.GetPersianValue();
                    if (!value.Contains(CatalogName.Trim().Trim('/').Trim()))
                        CatalogName += $"{value} / ";
                }
                CatalogName += string.Join(" - ", pages.Result.Select(x => x.CatalogNames.GetPersianValue()).Distinct());
                if (!CatalogName.Any(x => char.IsDigit(x)))
                {
                    CatalogName += " - " + pages.Result.FirstOrDefault()?.Number;
                }
                if (!IsOutsideOfBookTab)
                {
                    ApplicationReadingData.SetTitle(CatalogName);
                    ApplicationPagesData.Current.AddPageValue(PageType.Pages, CatalogStartPageNumber, BookId, CatalogId);
                }
            }
        }

        async Task<MessageContract<List<PageContract>>> FetchPage(bool isForce, long pageNumber, long bookId)
        {
            var pageResult = await TranslatorService.GetPageService(isForce).GetPageAsync(pageNumber, bookId);
            if (pageResult.IsSuccess && pageNumber == CatalogStartPageNumber && bookId == BookId)
                Page = pageResult.Result.First();
            return pageResult;
        }

        private async Task SwipeLeft()
        {
            if (IsLoading)
                return;
            if (CatalogStartPageNumber > 1)
            {
                CatalogStartPageNumber--;
                await LoadData();
            }
        }

        private async Task SwipeRight()
        {
            if (IsLoading)
                return;
            CatalogStartPageNumber++;
            await LoadData();
        }

        private async Task Touch(ParagraphModel paragraph)
        {
            if (!IsOnSelectionMode(paragraph))
            {
                await TouchBase(paragraph, false);
            }
        }

        private async Task RemoveReading()
        {
            if (IsOutsideOfBookTab)
                return;
            if (ApplicationReadingData.Current.Value.Items.Count == 0)
                await AlertHelper.Alert("خوانش", "در حال حاضر خوانشی وجود ندارد، به بخش خوانش ها مراجعه کنید.");
            else
            {
                var items = ApplicationReadingData.Current.Value.Items.Select(x => x.Name).Concat(new string[] { EmptyReading }).ToArray();
                var selected = await AlertHelper.Current.Display("خوانش", "انصراف", items);
                if (selected == EmptyReading)
                {
                    ApplicationReadingData.CurrentReadingData = null;
                }
                else
                {
                    var find = ApplicationReadingData.Current.Value.Items.FirstOrDefault(x => x.Name == selected);
                    if (find != null)
                    {
                        ApplicationReadingData.CurrentReadingData = find;
                        if (find.Pages.Any(x => x.PageType == Models.PageType.Pages))
                        {
                            await PageHelper.Clean();
                            await ApplicationPagesData.LoadStaticPageData(find);
                        }
                    }
                }
                OnPropertyChanged(nameof(ReadingName));
            }
        }

        private async Task SelectPage()
        {
            var data = await AlertHelper.DisplayPrompt("صفحات", "لطفا صفحه‌ی مورد نظر را انتخاب کنید.");
            data = FixArabicForSearch(data);
            if (int.TryParse(data, out int number))
            {
                CatalogStartPageNumber = number;
                await LoadData();
            }
        }

        private async Task SelectVerse()
        {
            var data = await AlertHelper.DisplayPrompt("انتخاب آیه", "لطفا شماره‌ی آیه‌ی مورد نظر را انتخاب کنید.");
            data = FixArabicForSearch(data);
            if (int.TryParse(data, out int number))
            {
                MessageContract<long> verseResult;
                try
                {
                    IsLoading = true;
                    verseResult = await TranslatorService.GetPageService(false).GetPageNumberByVerseNumberAsync(number, CatalogId);
                }
                finally
                {
                    IsLoading = false;
                }
                if (verseResult.IsSuccess)
                {
                    CatalogStartPageNumber = verseResult.Result;
                    await LoadData();
                }
                else
                    await AlertContract(verseResult);
            }
        }

        List<Stream> Audios { get; set; }
        int _AudioIndex = 0;
        double _LastPlaybackSpeedRatoSet = 0;
        bool isLoaded = false;
        private async Task Play()
        {
            try
            {
                if (IsLoading)
                    return;
                IsLoadingPlayStream = true;
                if (!isLoaded)
                {
                    var audios = GetAudios();
                    if (audios.Count == 0)
                    {
                        await AlertHelper.Display("ناموجود", "باشه");
                        return;
                    }

                    Audios = await DownloadAudios(audios);
                    var run = AudioPlayerBaseHelper.CurrentBase.Load(GetCopyStream(_AudioIndex));
                    AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = Current_PlaybackEnded;
                    PlaybackCurrentPosition = 0;
                }

                if (IsPlaying)
                    AudioPlayerBaseHelper.CurrentBase.Pause();
                else
                    PlayBase();
                IsPlaying = !IsPlaying;
                isLoaded = true;
            }
            finally
            {
                IsLoadingPlayStream = false;
            }
        }

        void PlayBase()
        {
            //In Android it will stop instead of pause
            if (_LastPlaybackSpeedRatoSet != _PlaybackSpeedRato)
            {
                _LastPlaybackSpeedRatoSet = _PlaybackSpeedRato;
                AudioPlayerBaseHelper.CurrentBase.SetSpeed(_PlaybackSpeedRato);
            }
            AudioPlayerBaseHelper.CurrentBase.Play();
        }

        List<AudioFileContract> GetAudios()
        {
            var result = Page.AudioFiles?.GroupBy(x => x.LanguageId).Select(x => x.FirstOrDefault()).FirstOrDefault();
            if (result == null)
                return Page.Paragraphs.Where(x => x.AudioFiles?.Count > 0).SelectMany(x => x.AudioFiles).ToList();
            return new List<AudioFileContract>()
            {
                result
            };
        }

        async Task<List<Stream>> DownloadAudios(List<AudioFileContract> audioFiles)
        {
            List<Stream> result = new List<Stream>();
            double duration = 0;
            foreach (var audio in audioFiles)
            {
                string key = $"{audio.PageId.GetValueOrDefault()}_{audio.ParagraphId.GetValueOrDefault()}_{audio.Id}";
                var saver = new ApplicationBookAudioData();
                saver.Initialize(key, ".mp3");
                var stream = await saver.DownloadFileStream($"{TranslatorService.ServiceAddress}/Storage/DownloadFile?fileId={audio.Id}&password={audio.Password}");
                result.Add(stream);
                duration += new TimeSpan(audio.DurationTicks).TotalSeconds;
            }
            _AudioDuration = duration;
            return result;
        }

        bool _canPositionUpdate = true;
        async Task PositionUpdater()
        {
            while (_canPositionUpdate)
            {
                AsyncHelper.RunOnUI(() =>
                {
                    if (IsPlaying && AudioPlayerBaseHelper.CurrentBase.CanSeek)
                    {
                        var currentPosition = GetToCurrentAudioIndexDuration(_AudioIndex).TotalSeconds;
                        _PlaybackCurrentPosition = 1 * ((currentPosition + AudioPlayerBaseHelper.CurrentBase.CurrentPosition) / _AudioDuration);

                        SetScrollByAudio();
                        OnPropertyChanged(nameof(PlaybackCurrentPosition));
                    }
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        TimeSpan GetToCurrentAudioIndexDuration(int index)
        {
            return new TimeSpan(Page.Paragraphs.Take(index).Sum(x => GetParagraphDuration(x).Ticks));
        }

        TimeSpan GetParagraphDuration(ParagraphContract paragraph)
        {
            var audios = paragraph?.AudioFiles;
            return new TimeSpan(audios?.Count > 0 ? audios.Sum(a => a.DurationTicks) : 0);
        }


        void SetScrollByAudio()
        {
            if (HasAutoScrollInPlayback)
            {
                int index = 0;
                var fullLength = Items.Sum(x => x.TranslatedValue.Length + x.MainDisplayValue.Length);
                var itemsLengths = Items.Select(x => new { Length = x.TranslatedValue.Length + x.MainDisplayValue.Length }); ;
                double from = 0;
                List<(double from, double to, int index)> PositionItems = new List<(double from, double to, int index)>();
                foreach (var item in Items)
                {
                    double to = from + (item.TranslatedValue.Length + item.MainDisplayValue.Length) / (double)fullLength;
                    PositionItems.Add((from, to, index));
                    from = to;
                    index++;
                }
                ScrollToIndex = PositionItems.Where(x => _PlaybackCurrentPosition > x.from && _PlaybackCurrentPosition < x.to).Select(x => x.index).FirstOrDefault();
            }
        }

        void Seek(double position)
        {
            if (Audios.Count > 1)
            {
                var selectedAudio = (int)(position / _AudioDuration * Audios.Count);
                if (selectedAudio == Audios.Count)
                    selectedAudio--;
                if (selectedAudio != _AudioIndex)
                {
                    PlayAudio(selectedAudio);
                }
                var paragraphTotal = GetParagraphDuration(Page.Paragraphs[selectedAudio]).TotalSeconds;
                var seek = paragraphTotal - (GetToCurrentAudioIndexDuration(selectedAudio).TotalSeconds + paragraphTotal - position);
                _PlaybackCurrentPosition = 1 * (position / _AudioDuration);
                AudioPlayerBaseHelper.CurrentBase.Seek(seek);
            }
            else
                AudioPlayerBaseHelper.CurrentBase.Seek(position);
        }

        void ResetPlayBack()
        {
            _AudioIndex = 0;
            isLoaded = false;
            IsPlaying = false;
            AudioPlayerBaseHelper.CurrentBase.Stop();
            _LastPlaybackSpeedRatoSet = 0;
        }

        private async Task Current_PlaybackEnded()
        {
            if (!IsPlaying)
                return;
            if (_AudioIndex == Audios.Count - 1)
            {
                try
                {
                    await SwipeRight();
                    await Play();
                }
                catch (Exception ex)
                {
                    await BaseViewModel.AlertExcepption(ex);
                }
            }
            else
            {
                PlayAudio(_AudioIndex + 1);
            }
        }

        void PlayAudio(int index)
        {
            _AudioIndex = index;
            bool doPlay = IsPlaying;
            IsPlaying = false;
            AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = null;
            AudioPlayerBaseHelper.CurrentBase.Stop();
            var run = AudioPlayerBaseHelper.CurrentBase.Load(GetCopyStream(index));
            AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = Current_PlaybackEnded;
            if (doPlay)
            {
                PlayBase();
                IsPlaying = !IsPlaying;
            }
        }

        MemoryStream GetCopyStream(int index)
        {
            var resultStream = new MemoryStream();
            Audios[index].Seek(0, SeekOrigin.Begin);
            Audios[index].CopyTo(resultStream);
            return resultStream;
        }

        private async Task LongTouched(ParagraphModel paragraphModel)
        {
            IsEnableMultipleSelection = !IsEnableMultipleSelection;
        }

        private async Task MultipleSelectOrUnSelect()
        {
            if (!IsEnableMultipleSelection)
                IsEnableMultipleSelection = true;
            var hasSelected = Items.Any(x => x.IsSelected);
            foreach (var item in Items)
            {
                item.IsSelected = !hasSelected;
            }
        }

        private async Task MultipleSelectionMenu()
        {
            if (!IsEnableMultipleSelection)
                await AlertHelper.Display("برای فعال سازی انتخاب چندتایی روی یکی از آیات انگشت خود را نگه دارید.", "باشه");
            else
                await Touch(null);
        }

        public void Dispose()
        {
            _canPositionUpdate = false;
            if (IsPlaying)
                ResetPlayBack();
        }
    }
}
