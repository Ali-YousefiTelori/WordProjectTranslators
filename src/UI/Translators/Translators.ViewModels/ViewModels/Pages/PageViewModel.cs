using System;
using System.Collections.Generic;
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

        public long PageId { get; set; }
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
                    AudioPlayerBaseHelper.CurrentBase.Seek(value * AudioPlayerBaseHelper.CurrentBase.Duration);
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
                PageId = pageResult.Result.First().Id;
            return pageResult;
        }

        private async Task SwipeLeft()
        {
            if (IsLoading)
                return;
            if (CatalogStartPageNumber > 1)
            {
                CatalogStartPageNumber--;
                ResetPlayBack();
                await LoadData();
            }
        }

        private async Task SwipeRight()
        {
            if (IsLoading)
                return;
            CatalogStartPageNumber++;
            ResetPlayBack();
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

        double _LastPlaybackSpeedRatoSet = 0;
        bool isLoaded = false;
        private async Task Play()
        {
            try
            {
                IsLoadingPlayStream = true;
                if (!isLoaded)
                {
                    string key = PageId.ToString();
                    var saver = new ApplicationBookAudioData();
                    saver.Initialize(key, ".mp3");
                    var stream = await saver.DownloadFileStream($"{TranslatorService.ServiceAddress}/Page/DownloadFile?pageId={key}");
                    var run = AudioPlayerBaseHelper.CurrentBase.Load(stream);
                    AudioPlayerBaseHelper.CurrentBase.PlaybackEnded = Current_PlaybackEnded;
                    PlaybackCurrentPosition = 0;
                }

                if (IsPlaying)
                    AudioPlayerBaseHelper.CurrentBase.Pause();
                else
                {
                    //In Android it will stop instead of pause
                    if (_LastPlaybackSpeedRatoSet != _PlaybackSpeedRato)
                    {
                        _LastPlaybackSpeedRatoSet = _PlaybackSpeedRato;
                        AudioPlayerBaseHelper.CurrentBase.SetSpeed(_PlaybackSpeedRato);
                    }
                    AudioPlayerBaseHelper.CurrentBase.Play();
                }
                IsPlaying = !IsPlaying;
                isLoaded = true;
            }
            finally
            {
                IsLoadingPlayStream = false;
            }
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
                        _PlaybackCurrentPosition = 1 * (AudioPlayerBaseHelper.CurrentBase.CurrentPosition / AudioPlayerBaseHelper.CurrentBase.Duration);

                        if (HasAutoScrollInPlayback)
                            ScrollToIndex = (int)(Items.Count * _PlaybackCurrentPosition);
                        OnPropertyChanged(nameof(PlaybackCurrentPosition));
                    }
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        void ResetPlayBack()
        {
            isLoaded = false;
            IsPlaying = false;
            AudioPlayerBaseHelper.CurrentBase.Stop();
            _LastPlaybackSpeedRatoSet = 0;
        }

        private async Task Current_PlaybackEnded()
        {
            if (!IsPlaying)
                return;
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
