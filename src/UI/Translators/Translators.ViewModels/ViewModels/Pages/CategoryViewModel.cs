using System;
using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

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
            SelectedName = category.Names.GetPersianValue();
            await PageHelper.PushPage(category.Id, 0, null, PageType.Book);
        }

        public long SelectedCategoryId { get; set; }
        public override async Task FetchData(bool isForce)
        {
            await FetchCategory(isForce);
        }

        public async Task FetchCategory(bool isForce)
        {
            var categories = await TranslatorService.GetBookServiceHttp(isForce).GetCategoriesAsync();
            if (categories.IsSuccess)
            {
                InitialData(categories.Result.Select(x => (CategoryModel)x));
            }
        }
    }
}
