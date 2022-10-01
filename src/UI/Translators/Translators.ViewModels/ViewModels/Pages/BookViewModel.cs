using System;
using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class BookViewModel : BaseCollectionViewModel<CategoryModel>
    {
        public BookViewModel()
        {
            TouchedCommand = CommandHelper.Create<CategoryModel>(Touched);
        }

        public ICommand<CategoryModel> TouchedCommand { get; set; }

        public async Task Touched(CategoryModel category)
        {
            await PageHelper.PushPage(category.Id, 0, null, PageType.Chapter, this);
        }

        public override void OnSelected(long id, long parentId)
        {
            var category = Items.FirstOrDefault(x => x.Id == id);
            if (category != null)
                SelectedName = category.Names.GetPersianValue();
        }

        public async Task Initialize(long id)
        {
            SelectedCategoryId = id;
            await LoadData();
        }

        public long SelectedCategoryId { get; set; }

        public override async Task FetchData(bool isForce = false)
        {
            await FetchBook(isForce, SelectedCategoryId);
        }

        public async Task FetchBook(bool isForce, long categoryId)
        {
            var books = await TranslatorService.GetBookService(isForce).FilterBooksAsync(categoryId);
            if (books.IsSuccess)
            {
                InitialData(books.Result.Select(x => (CategoryModel)x));
            }
        }

        protected override void OnDataInitilized()
        {
            if (string.IsNullOrEmpty(SelectedName))
                OnSelected(SelectedCategoryId, 0);
        }

        public override void Search()
        {
            var searchText = FixArabicForSearch(SearchText);
            Filter(x => x.Names.Any(n => FixArabicForSearch(n.Value).Contains(searchText)), x =>
            {
                var index = x.Names.Where(n => FixArabicForSearch(n.Value).Contains(searchText)).Select(i =>
                {
                    var value = FixArabicForSearch(i.Value);
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
