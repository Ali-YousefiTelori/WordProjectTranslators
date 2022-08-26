﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class PageViewModel : ParagraphBaseViewModel<ParagraphModel>
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
        }

        public ICommand<ParagraphModel> TouchedCommand { get; set; }
        public ICommand<ParagraphModel> LongTouchedCommand { get; set; }
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
            }
        }

        string _CatalogName;
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

        public async Task Initialize(long startPageNumber, long bookId, long catalogId)
        {
            CatalogId = catalogId;
            BookId = bookId;
            CatalogStartPageNumber = startPageNumber;
            _ = Task.Run(async () =>
            {
                await LoadData();
            });
        }

        public override async Task FetchData(bool isForce)
        {
            var pages = await FetchPage(isForce, CatalogStartPageNumber, BookId);
            //fetch next
            _ = Task.Run(async () =>
            {
                await FetchPage(isForce, CatalogStartPageNumber + 1, BookId);
            });
            //fetch prevoius
            _ = Task.Run(async () =>
            {
                await FetchPage(isForce, CatalogStartPageNumber - 1, BookId);
            });
            if (pages.IsSuccess)
            {
                InitialData(pages.Result.SelectMany(x => x.Paragraphs.Select(i => ParagraphModel.Map(i))));
                CatalogName = $"{GetSelectedTitleByType(typeof(BookViewModel))} / ";
                CatalogName += string.Join(" - ", pages.Result.Select(x => x.CatalogNames.GetPersianValue()).Distinct());
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
                await LoadData();
                ResetPlayBack();
            }
        }

        private async Task SwipeRight()
        {
            if (IsLoading)
                return;
            CatalogStartPageNumber++;
            await LoadData();
            ResetPlayBack();
        }

        private async Task Touch(ParagraphModel paragraph)
        {
            string displayName = null;
            try
            {
                IsLoading = true;
                var catalog = await TranslatorService.GetChapterService(false).GetChaptersAsync(paragraph.CatalogId);
                if (catalog.IsSuccess)
                    displayName = $"({LanguageValueBaseConverter.GetValue(catalog.Result.BookNames, false, "fa-ir")} {catalog.Result.Number}-{CleanArabicChars(LanguageValueBaseConverter.GetValue(catalog.Result.Names, false, "fa-ir"))} آیه‌ی {paragraph.Number})";
            }
            finally
            {
                IsLoading = false;
            }
            paragraph.DisplayName = displayName;
            await TouchBase(paragraph, false);
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

        bool isLoaded = false;
        private async Task Play()
        {
            try
            {
                IsLoadingPlayStream = true;
                if (!isLoaded)
                {
                    string key = PageId.ToString();
                    using WebClient client = new WebClient();
                    var saver = new ApplicationBookAudioData();
                    saver.Initialize(key, ".mp3");
                    var stream = await saver.DownloadFileStream($"http://api.noorpod.ir/Page/DownloadFile?pageId={key}");
                    var run = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Load(stream);
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.PlaybackEnded -= Current_PlaybackEnded;
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.PlaybackEnded += Current_PlaybackEnded;
                }

                if (IsPlaying)
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Pause();
                else
                    Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Play();
                IsPlaying = !IsPlaying;
                isLoaded = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsLoadingPlayStream = false;
            }
        }

        void ResetPlayBack()
        {
            isLoaded = false;
            IsPlaying = false;
            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current.Stop();
        }

        private async void Current_PlaybackEnded(object sender, EventArgs e)
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

            }
        }

        private async Task LongTouched(ParagraphModel paragraphModel)
        {

        }
    }
}
