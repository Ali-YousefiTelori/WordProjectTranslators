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
            //TouchedCommand = new Command<PageContract>(Touched);
        }

        public long CatalogStartPageNumber { get; set; }
        public Command<PageContract> TouchedCommand { get; set; }

        public async Task Initialize(long startPageNumber)
        {
            CatalogStartPageNumber = startPageNumber;
            await LoadData();
        }

        public override async Task FetchData()
        {
            var chapters = await TranslatorService.PageServiceHttp.GetPageAsync(CatalogStartPageNumber);
            if (chapters.IsSuccess)
            {
                InitialData(chapters.Result.SelectMany(x => x.Paragraphs).Select(x => (ParagraphModel)x));
            }
        }
    }
}
