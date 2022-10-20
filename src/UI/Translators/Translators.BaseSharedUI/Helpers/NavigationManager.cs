using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.UI.Pages;
using Translators.UI.Views;
using Translators.ViewModels;
using Translators.ViewModels.Pages;

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

        static INavigation SettingNavigation { get; set; }
        public static void InitializeSettingNavigation(INavigation navigation)
        {
            SettingNavigation = navigation;
        }

        public static Func<Page> GetCurrentPageFunc { get; set; }
        public static Page GetCurrentPage()
        {
#if (!SharedProject && !WPF)
            return App.Current.MainPage;//Navigation.NavigationStack.LastOrDefault();
#else
            return GetCurrentPageFunc?.Invoke();
#endif
        }



        public static async Task CleanPages()
        {
            foreach (var item in Navigation.NavigationStack.Where(x => x != null).Reverse().ToArray())
            {
                try
                {
                    await GetNavigation(item).PopAsync();
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

#if (WPF)
        static INavigation GetNavigation(Page page)
        {
            return null;
        }

        Page GetPage(UserControl content)
        {
            var page = new Page();
            page.DataContext = content.DataContext;
            page.Content = content;
            return page;
        }

        object GetDataContext(FrameworkElement framework)
        {
            return framework.DataContext;
        }

        public void SwitchPage(PageType pageType)
        {
            if (pageType == PageType.Category)
            {

            }
        }
#else
        static INavigation GetNavigation(Page page)
        {
            return page.Navigation;
        }

        Page GetPage(ContentView content)
        {
            var page = new ContentPage();
            page.BindingContext = content.BindingContext;
            page.Content = content;
            return page;
        }

        object GetDataContext(BindableObject bindableObject)
        {
            return bindableObject.BindingContext;
        }

        public void SwitchPage(PageType pageType)
        {
            if (pageType == PageType.Category)
            {
                ((AppShell)Shell.Current).TabBar.CurrentItem = ((AppShell)Shell.Current).TabBar.Items.First();
            }
        }
#endif
        public async Task<object> PushPage(long id, long rootId, object data, PageType pageType, BaseViewModel fromBaseViewModel)
        {
            Page page = null;
            switch (pageType)
            {
                case PageType.Book:
                    {
                        page = GetPage(new BookView());
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0, 0);
                        _ = (GetDataContext(page) as BookViewModel).Initialize(id);
                        break;
                    }
                case PageType.Chapter:
                    {
                        page = GetPage(new ChapterView());
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0, id);
                        _ = (GetDataContext(page) as ChapterViewModel).Initialize(id);
                        break;
                    }
                case PageType.Pages:
                    {
                        page = new PagesPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, rootId, (long)data);
                        _ = (GetDataContext(page) as PageViewModel).Initialize(id, rootId, (long)data);
                        break;
                    }
                case PageType.SearchResult:
                    {
                        page = GetPage(new SearchResultView());
                        (GetDataContext(page) as SearchResultPageViewModel).Initialize(data as List<SearchValueContract>);
                        break;
                    }
                case PageType.ParagraphResult:
                    {
                        page = GetPage(new ParagraphsView());
                        (GetDataContext(page) as ParagraphsPageViewModel).Initialize(data as List<SearchValueContract>);
                        break;
                    }
                case PageType.DoLinkPage:
                    {
                        page = GetPage(new LinkParagraphView());
                        (GetDataContext(page) as LinkParagraphPageViewModel).Initialize(data as ParagraphBaseModel[]);
                        break;
                    }
                case PageType.PagesFastRead:
                    {
                        page = new PagesPage();
                        (GetDataContext(page) as PageViewModel).SetIsOutsideOfBookTab();
                        _ = (GetDataContext(page) as PageViewModel).Initialize(id, rootId, (long)data);
                        break;
                    }
                case PageType.OfflineDownloadPage:
                    {
                        page = GetPage(new OfflineDownloaderView());
                        break;
                    }
            }


            if (page != null)
            {
                if (fromBaseViewModel is SettingPageViewModel)
                {
                    await SettingNavigation.PushAsync(page);
                }
                else
                {
                    if (GetDataContext(page) is BaseViewModel baseViewModel)
                    {
                        if (fromBaseViewModel != null)
                            baseViewModel.IsInSearchTab = fromBaseViewModel.IsInSearchTab;
                    }
                    if (fromBaseViewModel != null && fromBaseViewModel.IsInSearchTab)
                        await SearchNavigation.PushAsync(page);
                    else
                        await Navigation.PushAsync(page);
                }
            }
            return null;
        }

        public async Task Clean()
        {
            await CleanPages();
        }

        public async Task PopSettingPage()
        {
            await AsyncHelper.RunOnUI(async () => await SettingNavigation.PopAsync());
        }
    }
}
