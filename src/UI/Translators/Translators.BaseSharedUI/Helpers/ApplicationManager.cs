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
            System.Diagnostics.Process.Start($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpfcore.zip");
#elif (WPF)
            System.Diagnostics.Process.Process.Start($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpf.zip");
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

        public Task<long> GetBuildNumber()
        {
            if (long.TryParse(TranslatorService.GetCurrentBuildNumber(), out long result))
                return Task.FromResult(result);
            return Task.FromResult(0L);
        }

        public virtual Task KeepScreenOn(bool isKeepScreenOn)
        {
#if (Xamarin)
            if (isKeepScreenOn)
                DeviceDisplay.KeepScreenOn = isKeepScreenOn;
#endif
            return Task.CompletedTask;
        }
    }
}
