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

        public static async Task PushPage(long id, long rootId, PageType pageType)
        {
            await Current.PushPage(id, rootId, pageType);
        }
    }
}
