using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Contracts.Responses.Pages;
using Translators.Helpers;
using Translators.Logics;
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
            Player.Initalize(this, SwipeRight, SwipeLeft);
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

        AudioPlayerManager _Player = new AudioPlayerManager();
        public AudioPlayerManager Player
        {
            get
            {
                return _Player;
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
            _ = Task.Factory.StartNew(async () =>
            {
                await LoadData();
            });
        }



        public override async Task FetchData(bool isForce)
        {
            Player.Reset();
            if (isForce)
                Player.DoDownloadAgain = true;
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
                InitialData(pages.Result.Paragraphs.Select(i => ParagraphModel.Map(i, BookId, CatalogId, CatalogStartPageNumber)).Select(v =>
                {
                    v.IsEven = isEven;
                    isEven = !isEven;
                    return v;
                }));
                CatalogName = pages.Result.CatalogName;
                if (!IsOutsideOfBookTab)
                {
                    ApplicationReadingData.SetTitle(CatalogName);
                    ApplicationPagesData.Current.AddPageValue(PageType.Pages, CatalogStartPageNumber, BookId, CatalogId);
                }
            }
            Player.InitializeItems(Items);
        }

        async Task<MessageContract<PageResponseContract>> FetchPage(bool isForce, long pageNumber, long bookId)
        {
            var pageResult = await TranslatorService.GetPageService(isForce).GetPageAsync(new Contracts.Requests.Pages.PageRequest()
            {
                 PageNumber = pageNumber,
                 BookId = bookId,
            });
            if (pageResult.IsSuccess && pageNumber == CatalogStartPageNumber && bookId == BookId)
                Player.Page = pageResult.Result;
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
                    verseResult = await TranslatorService.GetOldPageService(false).GetPageNumberByVerseNumberAsync(number, CatalogId);
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
            if (IsLoading)
                return;
            await Player.PlayOrPause();
        }

        bool _canPositionUpdate = true;
        async Task PositionUpdater()
        {
            while (_canPositionUpdate)
            {
                AsyncHelper.RunOnUI(() =>
                {
                    Player.GoToPosition();
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
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
        }
    }
}
