using System.Threading.Tasks;
using Translators.Models.Interfaces;

namespace Translators.Helpers
{
    public class ClipboardHelper
    {
        public static IClipboardManager Current { get; set; }
        public static async Task CopyText(string text)
        {
            await Current.CopyText(text);
        }
    }
}
