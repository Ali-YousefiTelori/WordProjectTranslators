using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Converters;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class PageViewModel : BaseCollectionViewModel<ParagraphModel>
    {
        public PageViewModel()
        {
            SwipeLeftCommand = CommandHelper.Create(SwipeLeft);
            SwipeRightCommand = CommandHelper.Create(SwipeRight);
        }

        public ICommand<PageContract> TouchedCommand { get; set; }
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

        public override async Task FetchData()
        {
            var pages = await TranslatorService.PageServiceHttp.GetPageAsync(CatalogStartPageNumber, BookId);
            if (pages.IsSuccess)
            {
                InitialData(pages.Result.SelectMany(x => x.Paragraphs.Select(i => (ParagraphModel)i)));
                CatalogName = LanguageValueBaseConverter.GetValue(pages.Result.Last().CatalogNames, false, "fa-ir");
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
    }
}
