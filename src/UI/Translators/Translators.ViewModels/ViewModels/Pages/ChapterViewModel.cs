using System;
using System.Linq;
using System.Threading.Tasks;
using TranslatorApp.GeneratedServices;
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
            //SearchCommand = CommandHelper.Create(Search);
        }

        long BookId = 0;
        public ICommand<CatalogContract> TouchedCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public async Task Initialize(long id)
        {
            BookId = id;
            await LoadData();
        }

        public async Task Touched(CatalogContract catalog)
        {
            SelectedName = catalog.Names.GetPersianValue();
            await PageHelper.PushPage(catalog.StartPageNumber, catalog.BookId, catalog.Id, PageType.Pages, this);
        }

        public override async Task FetchData(bool isForce)
        {
            var chapters = await TranslatorService.GetChapterService(isForce).FilterChaptersAsync(BookId);
            if (chapters.IsSuccess)
            {
                InitialData(chapters.Result.OrderBy(x => x.Number));
            }
        }

        public override void Search()
        {
            var searchText = CleanText(SearchText);
            Filter(x => x.Names.Any(n => CleanText(n.Value).Contains(searchText)) || x.Number.ToString() == searchText, x =>
            {
                if (x.Number.ToString() == searchText)
                    return 0;
                var index = x.Names.Where(n => CleanText(n.Value).Contains(searchText)).Select(i =>
                {
                    var value = CleanText(i.Value);
                    if (value.StartsWith(searchText + " ") || value.StartsWith(searchText + "-") || value.StartsWith(searchText + " -"))
                        return 1;
                    else if (value.StartsWith(searchText))
                        return 10;
                    else
                        return 15;
                }).OrderBy(o => o).FirstOrDefault();
                return index;
            });
        }
    }
}
