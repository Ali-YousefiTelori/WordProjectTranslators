using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;

namespace Translators.ViewModels.Pages
{
    public class CategoryViewModel : BaseCollectionViewModel<CategoryModel>
    {
        public CategoryViewModel()
        {
            TouchedCommand = CommandHelper.Create<CategoryModel>(Touched);
            _ = LoadData();
        }

        public ICommand<CategoryModel> TouchedCommand { get; set; }

        public async Task Touched(CategoryModel category)
        {
            await PageHelper.PushPage(category.Id, 0, PageType.Book);
        }

        public long SelectedCategoryId { get; set; }
        public override async Task FetchData()
        {
            await FetchCategory();
        }

        public async Task FetchCategory()
        {
            var categories = await TranslatorService.BookServiceHttp.GetCategoriesAsync();
            if (categories.IsSuccess)
            {
                InitialData(categories.Result.Select(x => (CategoryModel)x));
            }
        }
    }
}
