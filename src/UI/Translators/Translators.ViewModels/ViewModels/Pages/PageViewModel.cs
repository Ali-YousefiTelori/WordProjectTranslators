using Microsoft.Maui.Controls;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;

namespace Translators.ViewModels.Pages
{
    public class PageViewModel : BaseCollectionViewModel<ParagraphModel>
    {
        public PageViewModel()
        {
            SwipeLeftCommand = new Command(SwipeLeft);
            SwipeRightCommand = new Command(SwipeRight);
        }

        public Command<PageContract> TouchedCommand { get; set; }
        public Command SwipeLeftCommand { get; set; }
        public Command SwipeRightCommand { get; set; }

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

        public async Task Initialize(long startPageNumber)
        {
            CatalogStartPageNumber = startPageNumber;
            await LoadData();
        }

        public override async Task FetchData()
        {
            var pages = await TranslatorService.PageServiceHttp.GetPageAsync(CatalogStartPageNumber);
            if (pages.IsSuccess)
            {
                InitialData(pages.Result.Paragraphs.Select(x => (ParagraphModel)x));
                CatalogName = pages.Result.CatalogNames.FirstOrDefault()?.Value;
            }
        }

        private async void SwipeLeft()
        {
            if (IsLoading)
                return;
            if (CatalogStartPageNumber > 1)
            {
                CatalogStartPageNumber--;
                await LoadData();
            }
        }

        private async void SwipeRight()
        {
            if (IsLoading)
                return;
            CatalogStartPageNumber++;
            await LoadData();
        }
    }
}
