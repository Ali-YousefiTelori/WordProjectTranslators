using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models.Contracts;
using Translators.ServiceManagers;

namespace Translators.Engines.OfflineDownloaders
{
    public class ChapterServiceOfflineDownloader : ServiceOfflineDownloaderBase
    {
        public ChapterServiceOfflineDownloader()
        {
            ServiceAddress = $"{TranslatorService.ServiceAddress}/Chapter/GetOfflineCache";
            SaveToFileAddress = Path.Combine(GetFolderPath(), "ChapterServiceOffline.zip");
        }

        public override async Task<bool> Extract()
        {
            if (File.Exists(SaveToFileAddress))
            {
                var result = DeserializeFromFile<List<CatalogContract>>();
                var catalogGroups = result.GroupBy(x => x.BookId).ToList();
                CalculateLength(catalogGroups.Count + result.Count);

                foreach (var catalogGroup in catalogGroups)
                {
                    await ClientConnectionManager.SaveLocal("chapter/FilterChapters",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "bookId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(catalogGroup.Key) } },
                        JsonConvert.SerializeObject(ToMessageContract(catalogGroup)));
                    AddProgress();
                }

                foreach (var catalog in result)
                {
                    await ClientConnectionManager.SaveLocal("chapter/GetChapters",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "chapterId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(catalog.Id) } },
                        JsonConvert.SerializeObject(ToMessageContract(catalog)));
                    AddProgress();
                }
            }
            return true;
        }
    }
}