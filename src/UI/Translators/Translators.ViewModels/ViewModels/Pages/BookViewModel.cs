using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class BookViewModel : BaseCollectionViewModel<CategoryModel>
    {
        public BookViewModel()
        {
            TouchedCommand = CommandHelper.Create<CategoryModel>(Touched);
            _ = LoadData();
        }

        public ICommand<CategoryModel> TouchedCommand { get; set; }

        public async Task Touched(CategoryModel category)
        {
            await PageHelper.PushPage(category.Id, 0, PageType.Sura);
        }

        public async Task Initialize(long id)
        {
            SelectedCategoryId = id;
            await LoadData();
        }

        public long SelectedCategoryId { get; set; }

        public override async Task FetchData()
        {
            await FetchBook(SelectedCategoryId);
        }

        public async Task FetchBook(long categoryId)
        {
            var books = await TranslatorService.BookServiceHttp.FilterBooksAsync(categoryId);
            if (books.IsSuccess)
            {
                InitialData(books.Result.Select(x => (CategoryModel)x));
            }
        }
    }
}
