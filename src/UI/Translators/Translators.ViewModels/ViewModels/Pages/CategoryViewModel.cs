using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models;

namespace Translators.ViewModels.Pages
{
    public class CategoryViewModel : BaseCollectionViewModel<CategoryModel>
    {
        public CategoryViewModel()
        {
            TouchedCommand = new Command<CategoryModel>(Touched);
            _ = LoadData();
        }

        public Command<CategoryModel> TouchedCommand { get; set; }

        public async void Touched(CategoryModel category)
        {
            if (category.Type == ServiceType.Category)
                ServiceType = ServiceType.Book;
            else if (category.Type == ServiceType.Book)
            {
                await PageHelper.PushPage(category.Id, PageType.Sura);
                return;
            }
            SelectedCategoryId = category.Id;
            await LoadData();
        }

        ServiceType ServiceType { get; set; } = ServiceType.Category;
        public long SelectedCategoryId { get; set; }
        public override async Task FetchData()
        {
            if (ServiceType == ServiceType.Category)
                await FetchCategory();
            else if (ServiceType == ServiceType.Book)
                await FetchBook(SelectedCategoryId);
        }

        public async Task FetchCategory()
        {
            var categories = await TranslatorService.BookServiceHttp.GetCategoriesAsync();
            if (categories.IsSuccess)
            {
                InitialData(categories.Result.Select(x => (CategoryModel)x));
            }
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
