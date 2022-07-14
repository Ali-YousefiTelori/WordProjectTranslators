using System.Threading.Tasks;
using Translators.Models;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ViewModels;

namespace Translators.Helpers
{
    public class PageHelper
    {
        public static IPageManager Current { get; set; }
        public static void Initialize(IPageManager navigation)
        {
            Current = navigation;
        }

        public static async Task<object> PushPage(long id, long rootId, object data, PageType pageType, BaseViewModel fromBaseViewModel)
        {
            return await Current.PushPage(id, rootId, data, pageType, fromBaseViewModel);
        }

        public static void SwitchPage(PageType pageType)
        {
            Current.SwitchPage(pageType);
        }

        public static async Task Clean()
        {
            ApplicationReadingData.IsSwitchingToNewReading = true;
            await Current.Clean();
        }
    }
}
