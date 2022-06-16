using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void RemovePages(Type type)
        {
            foreach (var page in Navigation.NavigationStack.Where(x => x != null && x.GetType() == type))
            {
                Navigation.RemovePage(page);
            }
        }

        public async Task PushPage(long id, long rootId, PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Book:
                    {
                        var page = new BookPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0);
                        _ = (page.BindingContext as BookViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Chapter:
                    {
                        var page = new ChapterPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, 0);
                        _ = (page.BindingContext as ChapterViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Pages:
                    {
                        var page = new PagesPage();
                        ApplicationPagesData.Current.AddPageValue(pageType, id, rootId);
                        _ = (page.BindingContext as PageViewModel).Initialize(id, rootId);
                        await Navigation.PushAsync(page);
                        break;
                    }
            }
        }
    }
}
