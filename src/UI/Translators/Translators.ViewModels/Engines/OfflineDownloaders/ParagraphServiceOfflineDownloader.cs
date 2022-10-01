using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.ServiceManagers;

namespace Translators.Engines.OfflineDownloaders
{
    public class ParagraphServiceOfflineDownloader : ServiceOfflineDownloaderBase
    {
        public ParagraphServiceOfflineDownloader()
        {
            ServiceAddress = $"{TranslatorService.ServiceAddress}/Paragraph/GetOfflineCache";
            SaveToFileAddress = Path.Combine(GetFolderPath(), "ParagraphServiceOffline.zip");
        }

        public override async Task<bool> Extract()
        {
            if (File.Exists(SaveToFileAddress))
            {
               
            }
            return true;
        }
    }
}