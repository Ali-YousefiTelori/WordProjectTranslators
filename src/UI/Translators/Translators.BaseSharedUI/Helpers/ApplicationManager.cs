using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
            Process.Start($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpfcore.zip");
#elif (WPF)
            Process.Start($"{TranslatorService.ServiceAddress}/Application/DownloadLastVersion?fileName=wpf.zip");
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

        public Task KeepScreenOn(bool isKeepScreenOn)
        {
#if (Xamarin)
            DeviceDisplay.KeepScreenOn = isKeepScreenOn;
#endif
            return Task.CompletedTask;
        }
    }
}
