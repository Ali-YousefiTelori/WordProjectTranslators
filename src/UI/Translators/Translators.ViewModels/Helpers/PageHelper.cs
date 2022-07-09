using System.Threading.Tasks;
using Translators.Models;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public class PageHelper
    {
        public static IPageManager Current { get; set; }
        public static void Initialize(IPageManager navigation)
        {
            Current = navigation;
        }

        public static async Task<object> PushPage(long id, long rootId, object data, PageType pageType, bool isFromSearchPage = false)
        {
            return await Current.PushPage(id, rootId, data, pageType);
        }

        public static async Task Clean()
        {
            await Current.Clean();
        }
    }
}
