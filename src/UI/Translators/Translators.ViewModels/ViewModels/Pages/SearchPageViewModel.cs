using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class SearchModel : CategoryModel
    {
        int _Left = 20;
        public int Left
        {
            get => _Left;
            set
            {
                _Left = value;
                OnPropertyChanged(nameof(Left));
            }
        }

        public long CategoryId { get; set; }

        public static implicit operator SearchModel(BookContract book)
        {
            return new SearchModel()
            {
                Id = book.Id,
                Names = book.Names,
                Type = ServiceType.Book,
                CategoryId = book.CategoryId
            };
        }
    }

    public class SearchPageViewModel : BaseCollectionViewModel<SearchModel>
    {
        public SearchPageViewModel()
        {
            SearchCommand = CommandHelper.Create(DoSearch);
            _ = LoadData();
        }

        public ICommand SearchCommand { get; set; }

        public bool IsSearchInMainText { get; set; } = true;
        public bool IsSearchInTranslateText { get; set; } = true;
        public bool DoFullWordsSearch { get; set; }
        
        public override string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public async Task DoSearch()
        {
            try
            {
                IsLoading = true;
                var bookIds = Items.Where(x => x.IsSelected).Select(x => x.Id).Distinct().ToList();
                var result = await TranslatorService.GetPageService(true).SearchAsync(new Contracts.Requests.AdvancedSearchFilterRequestContract()
                {
                    BookIds = bookIds,
                    Search = SearchText,
                    SkipSearchInMain = !IsSearchInMainText,
                    SkipSearchInTranslates = !IsSearchInTranslateText,
                    DoFullWordsSearch = DoFullWordsSearch
                });
                if (result.IsSuccess)
                    await PageHelper.PushPage(0, 0, result.Result, PageType.SearchResult);
                else
                    await AlertContract(result);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override async Task FetchData(bool isForce = false)
        {
            await FetchBook(isForce);
        }

        public async Task FetchBook(bool isForce)
        {
            var books = await TranslatorService.GetBookService(isForce).GetBooksAsync();
            if (books.IsSuccess)
            {
                List<SearchModel> items = new List<SearchModel>();
                foreach (var group in books.Result.Select(x => (SearchModel)x).GroupBy(x => x.CategoryId))
                {
                    var searchModel = new SearchModel()
                    {
                        IsSelected = true,
                        Type = ServiceType.Book,
                        Left = 0,
                        Names = new List<ValueContract>()
                        {
                             new ValueContract()
                             {
                                  Language = new LanguageContract()
                                  {
                                       Code = "fa-ir"
                                  },
                                  Value = GetBookName(group)
                             }
                        },
                        CategoryId = group.First().CategoryId
                    };
                    searchModel.SelectionChanged = () =>
                    {
                        foreach (var item in group.ToList())
                        {
                            item.IsSelected = searchModel.IsSelected;
                        }
                    };
                    items.Add(searchModel);
                    foreach (var item in group.ToList())
                    {
                        var mapped = item;
                        items.Add(mapped);
                    }
                }
                InitialData(items);
            }
        }

        string GetBookName(IGrouping<long, SearchModel> books)
        {
            if (books.Any(x => x.Names.Any(n => CleanText(n.Value).Contains("قران"))))
                return "قرآن";
            else if (books.Any(x => x.Names.Any(n => CleanText(n.Value).Contains("مرقس"))))
                return "انجیل";
            return "تورات";
        }
    }
}