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
            await PageHelper.PushPage(category.Id, 0, PageType.Sura);
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
            var books = await TranslatorService.GetBookServiceHttp(isForce).FilterBooksAsync(categoryId);
            if (books.IsSuccess)
            {
                InitialData(books.Result.Select(x => (CategoryModel)x));
            }
        }
    }
}
