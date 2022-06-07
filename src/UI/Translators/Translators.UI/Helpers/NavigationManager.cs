using Translators.Helpers;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.UI.Views.Pages;
using Translators.ViewModels.Pages;

namespace Translators.UI.Helpers
{
    public class NavigationManager : IPageManager
    {
        static NavigationManager()
        {
            PageHelper.Initialize(Current);
        }

        public static NavigationManager Current { get; set; } = new NavigationManager();
        static INavigation Navigation { get; set; }
        public static void Initialize(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task PushPage(long id, PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Sura:
                    {
                        var page = new ChapterPage();
                        _ = (page.BindingContext as ChapterViewModel).Initialize(id);
                        await Navigation.PushAsync(page);
                        break;
                    }
                case PageType.Ayat:
                    break;
            }
        }
    }
}
