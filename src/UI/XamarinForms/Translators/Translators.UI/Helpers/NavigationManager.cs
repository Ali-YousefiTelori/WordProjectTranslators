using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Models;
using Translators.Models.Interfaces;
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
                        _ = (page.BindingContext as BookViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Sura:
                    {
                        var page = new ChapterPage();
                        _ = (page.BindingContext as ChapterViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Ayat:
                    {
                        var page = new PagesPage();
                        _ = (page.BindingContext as PageViewModel).Initialize(id, rootId);
                        await Navigation.PushAsync(page);
                        break;
                    }
            }
        }
    }
}
