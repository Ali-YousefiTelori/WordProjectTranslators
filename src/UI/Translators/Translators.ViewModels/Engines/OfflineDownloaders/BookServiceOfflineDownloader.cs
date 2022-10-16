using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Translators.Models.Contracts;
using Translators.ServiceManagers;

namespace Translators.Engines.OfflineDownloaders
{
    public class BookServiceOfflineDownloader : ServiceOfflineDownloaderBase
    {
        public BookServiceOfflineDownloader()
        {
            ServiceAddress = $"{TranslatorService.ServiceAddress}/Book/GetOfflineCache";
            SaveToFileAddress = Path.Combine(GetFolderPath(), "BookServiceOffline.zip");
        }

        public override async Task<bool> Extract()
        {
            if (File.Exists(SaveToFileAddress))
            {
                var result = DeserializeFromFile<BookServiceModelsContract>();
                await ClientConnectionManager.SaveLocal("book/GetCategories", null, JsonConvert.SerializeObject(ToMessageContract(result.Categories)));
                foreach (var bookGroup in result.Books.GroupBy(x => x.CategoryId))
                {
                    await ClientConnectionManager.SaveLocal("book/FilterBooks",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "categoryId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(bookGroup.Key) } },
                        JsonConvert.SerializeObject(ToMessageContract(bookGroup.ToList())));
                }

                foreach (var book in result.Books)
                {
                    await ClientConnectionManager.SaveLocal("book/GetCategoryByBookId",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "bookId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(book.Id) } },
                        JsonConvert.SerializeObject(ToMessageContract(result.Categories.FirstOrDefault(x => x.Id == book.CategoryId))));

                    await ClientConnectionManager.SaveLocal("book/GetBookById",
                        new SignalGo.Shared.Models.ParameterInfo[] { new SignalGo.Shared.Models.ParameterInfo() { Name = "bookId", Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(book.Id) } },
                        JsonConvert.SerializeObject(ToMessageContract(book)));
                }
                await ClientConnectionManager.SaveLocal("book/GetBooks", null, JsonConvert.SerializeObject(ToMessageContract(result.Books)));
            }
            return true;
        }
    }
}
