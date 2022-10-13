using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.ServiceManagers;

namespace Translators.Engines.OfflineDownloaders
{
    public class PageServiceOfflineDownloader : ServiceOfflineDownloaderBase
    {
        public PageServiceOfflineDownloader()
        {
            ServiceAddress = $"{TranslatorService.ServiceAddress}/Page/GetOfflineCache";
            SaveToFileAddress = Path.Combine(GetFolderPath(), "PageServiceOffline.zip");
        }

        public override async Task<bool> Extract()
        {
            if (File.Exists(SaveToFileAddress))
            {
                var result = DeserializeFromFile<List<PageContract>>();

                var pageGroups = result.GroupBy(x => new { x.Number, x.BookId }).ToList();
                var bookGroup = result.GroupBy(x => x.BookId).ToList();
                var catalogGroup = result.GroupBy(x => x.CatalogId).ToList();
                CalculateLength(pageGroups.Count + bookGroup.Count + catalogGroup.Count);
                foreach (var page in pageGroups)
                {
                    await ClientConnectionManager.SaveLocal("page/GetPage",
                        new SignalGo.Shared.Models.ParameterInfo[]
                        {
                            new SignalGo.Shared.Models.ParameterInfo() { Name = "pageNumber", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(page.Key.Number) },
                            new SignalGo.Shared.Models.ParameterInfo() { Name = "bookId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(page.Key.BookId) }
                        }, JsonConvert.SerializeObject(ToMessageContract(page.ToList())));
                    AddProgress();
                }

                foreach (var page in bookGroup)
                {
                    await ClientConnectionManager.SaveLocal("page/GetPagesByBookId",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "bookId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(page.Key) } },
                        JsonConvert.SerializeObject(ToMessageContract(page)));
                    AddProgress();
                }

                foreach (var page in catalogGroup)
                {
                    foreach (var paragraph in page.SelectMany(x => x.Paragraphs))
                    {
                        var findPage = page.FirstOrDefault(x => x.Id == paragraph.PageId);
                        await ClientConnectionManager.SaveLocal("page/GetPageNumberByVerseNumber",
                        new SignalGo.Shared.Models.ParameterInfo[]
                        {
                            new SignalGo.Shared.Models.ParameterInfo() { Name = "verseNumber", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(paragraph.Number) },
                            new SignalGo.Shared.Models.ParameterInfo() { Name = "catalogId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(page.Key) }
                        }, JsonConvert.SerializeObject(ToMessageContract(findPage.Number)));
                    }
                    AddProgress();
                }
            }
            return true;
        }
    }
}