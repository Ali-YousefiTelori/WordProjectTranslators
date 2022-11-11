using System.Diagnostics;
using System.Runtime.InteropServices;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.UI.Helpers
{
    public class ApplicationManager : IApplicationManager
    {
        public async Task DownloadNewVersion()
        {
            string fileName = "";
#if (CSHTML5)

#elif (WPFCore)
            OpenUrl($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpfcore.zip");
#elif (WPF)
            OpenUrl($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpf.zip");
#else
            if (Device.RuntimePlatform == Device.iOS)
            {
                //iOS stuff
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                fileName = "noorpod.ir.translators.apk";
            }
            await Share.RequestAsync($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName={fileName}");
#endif
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
#if (NET45)
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
#else
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
#endif
            }
        }

        public Task<long> GetBuildNumber()
        {
            if (long.TryParse(TranslatorService.GetCurrentBuildNumber(), out long result))
                return Task.FromResult(result);
            return Task.FromResult(0L);
        }

        public virtual async Task KeepScreenOn(bool isKeepScreenOn)
        {
#if (Xamarin)
            if (isKeepScreenOn)
                DeviceDisplay.KeepScreenOn = isKeepScreenOn;
#endif
        }
    }
}
