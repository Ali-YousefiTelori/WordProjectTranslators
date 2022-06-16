using System.Threading.Tasks;
using Translators.Models.Interfaces;
using Xamarin.Essentials;

namespace Translators.UI.Helpers
{
    public class ClipboardManager : IClipboardManager
    {
        public async Task CopyText(string text)
        {
            await Clipboard.SetTextAsync(text);
        }
    }
}
