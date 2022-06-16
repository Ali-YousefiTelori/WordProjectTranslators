using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class ChapterViewModel : BaseCollectionViewModel<CatalogContract>
    {
        public ChapterViewModel()
        {
            TouchedCommand = CommandHelper.Create<CatalogContract>(Touched);
        }

        long BookId = 0;
        public ICommand<CatalogContract> TouchedCommand { get; set; }

        public async Task Initialize(long id)
        {
            BookId = id;
            await LoadData();
        }

        public async Task Touched(CatalogContract catalog)
        {
            await PageHelper.PushPage(catalog.StartPageNumber, catalog.BookId, catalog, PageType.Pages);
        }

        public override async Task FetchData(bool isForce)
        {
            var chapters = await TranslatorService.GetChapterServiceHttp(isForce).FilterChaptersAsync(BookId);
            if (chapters.IsSuccess)
            {
                InitialData(chapters.Result);
            }
        }
    }
}
