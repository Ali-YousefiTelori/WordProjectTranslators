using Translators.Models.Interfaces;

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
