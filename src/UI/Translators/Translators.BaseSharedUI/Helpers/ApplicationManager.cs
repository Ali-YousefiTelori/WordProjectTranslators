using System;
using System.Collections.Generic;
using System.Text;
using Translators.Models.Interfaces;
using Translators.ServiceManagers;

namespace Translators.UI.Helpers
{
    public class ApplicationManager : IApplicationManager
    {
        public async Task DownloadNewVersion()
        {
            string fileName = "";
#if (WPF)

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
            if (long.TryParse(TranslatorService.GetCurrentVersionNumber(), out long result))
                return Task.FromResult(result);
            return Task.FromResult(0L);
        }
    }
}
