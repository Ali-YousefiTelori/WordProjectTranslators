using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.UI.Views.Pages;
using Translators.ViewModels;
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

        static INavigation SettingNavigation { get; set; }
        public static void InitializeSettingNavigation(INavigation navigation)
        {
            SettingNavigation = navigation;
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

        public async Task<object> PushPage(long id, long rootId, object data, PageType pageType, BaseViewModel fromBaseViewModel)
        {
            Page page = null;
            switch (pageType)
            {
                case PageType.Book:
                    {
                        page = new BookPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0, 0);
                        _ = (page.BindingContext as BookViewModel).Initialize(id);
                        break;
                    }
                case PageType.Chapter:
                    {
                        page = new ChapterPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0, id);
                        await BookViewModel.OnSelectedTitleByType(typeof(BookViewModel), id, 0);
                        _ = (page.BindingContext as ChapterViewModel).Initialize(id);
                        break;
                    }
                case PageType.Pages:
                    {
                        page = new PagesPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, rootId, (long)data);
                        _ = (page.BindingContext as PageViewModel).Initialize(id, rootId, (long)data);
                        break;
                    }
                case PageType.SearchResult:
                    {
                        page = new SearchResultPage();
                        (page.BindingContext as SearchResultPageViewModel).Initialize(data as List<SearchValueContract>);
                        break;
                    }
                case PageType.ParagraphResult:
                    {
                        page = new ParagraphsPage();
                        (page.BindingContext as ParagraphsPageViewModel).Initialize(data as List<SearchValueContract>);
                        break;
                    }
                case PageType.DoLinkPage:
                    {
                        page = new LinkParagraphPage();
                        (page.BindingContext as LinkParagraphPageViewModel).Initialize(data as ParagraphBaseModel);
                        break;
                    }
                case PageType.PagesFastRead:
                    {
                        page = new PagesPage();
                        (page.BindingContext as PageViewModel).SetIsOutsideOfBookTab();
                        _ = (page.BindingContext as PageViewModel).Initialize(id, rootId, (long)data);
                        break;
                    }
                case PageType.OfflineDownloadPage:
                    {
                        page = new OfflineDownloaderPage();
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
                    if (page.BindingContext is BaseViewModel baseViewModel)
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

        public void SwitchPage(PageType pageType)
        {
            if (pageType == PageType.Category)
            {
                ((AppShell)Shell.Current).TabBar.CurrentItem = ((AppShell)Shell.Current).TabBar.Items.First();
            }
        }

        public async Task PopSettingPage()
        {
            Device.BeginInvokeOnMainThread(async () => await SettingNavigation.PopAsync());
        }
    }
}
