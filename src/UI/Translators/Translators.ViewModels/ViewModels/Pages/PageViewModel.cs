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
        }

        public ICommand<ParagraphModel> TouchedCommand { get; set; }
        public ICommand SwipeLeftCommand { get; set; }
        public ICommand SwipeRightCommand { get; set; }

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
            var pages = await TranslatorService.GetPageServiceHttp(isForce).GetPageAsync(CatalogStartPageNumber, BookId);
            if (pages.IsSuccess)
            {
                InitialData(pages.Result.SelectMany(x => x.Paragraphs.Select(i => ParagraphModel.Map(i))));
                CatalogName = LanguageValueBaseConverter.GetValue(pages.Result.Last().CatalogNames, false, "fa-ir");
                ApplicationPagesData.Current.AddPageValue(PageType.Pages, CatalogStartPageNumber, BookId);
            }
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
    }
}
