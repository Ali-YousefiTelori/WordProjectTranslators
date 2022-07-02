using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.UI.Views.Pages;
using Translators.ViewModels.Pages;
using Xamarin.Forms;

namespace Translators.UI.Helpers
{
    public class NavigationManager : IPageManager
    {
        public static NavigationManager Current { get; set; } = new NavigationManager();
        static INavigation Navigation { get; set; }
        public static void Initialize(INavigation navigation)
        {
            Navigation = navigation;
        }

        static INavigation SearchNavigation { get; set; }
        public static void InitializeSearchNavigation(INavigation navigation)
        {
            SearchNavigation = navigation;
        }

        public static Page GetCurrentPage()
        {
            return App.Current.MainPage;//Navigation.NavigationStack.LastOrDefault();
        }

        public static async Task CleanPages()
        {
            foreach (var item in Navigation.NavigationStack.Where(x => x != null).Reverse().ToArray())
            {
                try
                {
                    await item.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void RemovePages(Type type)
        {
            foreach (var page in Navigation.NavigationStack.Where(x => x != null && x.GetType() == type))
            {
                Navigation.RemovePage(page);
            }
        }

        public async Task<object> PushPage(long id, long rootId, object data, PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Book:
                    {
                        var page = new BookPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0,0);
                        _ = (page.BindingContext as BookViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Chapter:
                    {
                        var page = new ChapterPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0, id);
                        BookViewModel.OnSelectedTitleByType(typeof(BookViewModel), id, 0);
                        await (page.BindingContext as ChapterViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Pages:
                    {
                        var page = new PagesPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, rootId, (long)data);
                        _ = (page.BindingContext as PageViewModel).Initialize(id, rootId, (long)data);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.SearchResult:
                    {
                        var page = new SearchResultPage();
                        (page.BindingContext as SearchResultPageViewModel).Initialize(data as List<SearchValueContract>);
                        await SearchNavigation.PushAsync(page);
                        break;
                    }
            }
            return null;
        }

        public async Task Clean()
        {
            await CleanPages();
        }
    }
}
