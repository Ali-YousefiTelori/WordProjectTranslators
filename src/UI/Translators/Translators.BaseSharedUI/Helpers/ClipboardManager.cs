using Translators.Models.Interfaces;

namespace Translators.UI.Helpers
{
    public class ClipboardManager : IClipboardManager
    {
        public async Task CopyText(string text)
        {
#if (WPF)
            Clipboard.SetText(text);
#else
            await Clipboard.SetTextAsync(text);
#endif
        }
    }
}
