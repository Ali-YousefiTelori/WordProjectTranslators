using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.DataTypes;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class PageViewModel : BaseCollectionViewModel<ParagraphModel>
    {
        public PageViewModel()
        {
            SwipeLeftCommand = CommandHelper.Create(SwipeLeft);
            SwipeRightCommand = CommandHelper.Create(SwipeRight);
            TouchedCommand = CommandHelper.Create<ParagraphModel>(Touch);
            RemoveReadingCommand = CommandHelper.Create(RemoveReading);
            SelectPageCommand = CommandHelper.Create(SelectPage);
            SelectVerseCommand = CommandHelper.Create(SelectVerse);
        }

        public ICommand<ParagraphModel> TouchedCommand { get; set; }
        public ICommand SwipeLeftCommand { get; set; }
        public ICommand SwipeRightCommand { get; set; }
        public ICommand RemoveReadingCommand { get; set; }
        public ICommand SelectPageCommand { get; set; }
        public ICommand SelectVerseCommand { get; set; }
        
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
                if (ApplicationReadingData.CurrentReadingData == null)
                    return EmptyReading;
                return ApplicationReadingData.CurrentReadingData.Name;
            }
        }

        public long BookId { get; set; }
        public CatalogContract Catalog { get; set; }

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

        public async Task Initialize(long startPageNumber, long bookId)
        {
            BookId = bookId;
            CatalogStartPageNumber = startPageNumber;
            await LoadData();
        }

        public override async Task FetchData(bool isForce)
        {
            var pages = await FetchPage(isForce, CatalogStartPageNumber, BookId);
            //fetch next
            _ = FetchPage(isForce, CatalogStartPageNumber + 1, BookId);
            //fetch prevoius
            _ = FetchPage(isForce, CatalogStartPageNumber - 1, BookId);
            if (pages.IsSuccess)
            {
                InitialData(pages.Result.SelectMany(x => x.Paragraphs.Select(i => ParagraphModel.Map(i))));
                CatalogName = $"{GetSelectedTitleByType(typeof(BookViewModel))} / ";
                CatalogName += string.Join(" - ", pages.Result.Select(x => x.CatalogNames.GetPersianValue()).Distinct());
                ApplicationReadingData.SetTitle(CatalogName);
                ApplicationPagesData.Current.AddPageValue(PageType.Pages, CatalogStartPageNumber, BookId);
            }
        }

        async Task<MessageContract<List<PageContract>>> FetchPage(bool isForce, long pageNumber, long bookId)
        {
            return await TranslatorService.GetPageServiceHttp(isForce).GetPageAsync(pageNumber, bookId);
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
            string displayName = null;
            _ = Task.Run(async () =>
            {
                var catalog = await TranslatorService.GetChapterServiceHttp(false).GetChaptersAsync(paragraph.CatalogId);
                if (catalog.IsSuccess)
                    displayName = $"({catalog.Result.Number}- {LanguageValueBaseConverter.GetValue(catalog.Result.BookNames, false, "fa-ir")} آیه {paragraph.Number})";
            });

            var selectedType = await AlertHelper.Display<VerseRightClickType>("عملیات", "انصراف", "کپی همه", "کپی آیه", "کپی ترجمه");
            switch (selectedType)
            {
                case VerseRightClickType.CopyAll:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(paragraph.MainValue);
                        stringBuilder.Append(paragraph.TranslatedValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyVerse:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraph.MainValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                case VerseRightClickType.CopyTranslate:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(paragraph.TranslatedValue);
                        stringBuilder.Append(displayName);
                        await ClipboardHelper.CopyText(stringBuilder.ToString());
                        break;
                    }
                default:
                    break;
            }
        }

        private async Task RemoveReading()
        {
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
            if (int.TryParse(data, out int number))
            {
                CatalogStartPageNumber = number;
                await LoadData();
            }
        }

        private async Task SelectVerse()
        {
            var data = await AlertHelper.DisplayPrompt("انتخاب آیه", "لطفا شماره‌ی آیه‌ی مورد نظر را انتخاب کنید.");
            if (int.TryParse(data, out int number))
            {
                var verseResult = await TranslatorService.GetPageServiceHttp(false).GetPageNumberByVerseNumberAsync(number, BookId);
                if (verseResult.IsSuccess)
                {
                    CatalogStartPageNumber = verseResult.Result;
                    await LoadData();
                }
            }
        }
    }
}
