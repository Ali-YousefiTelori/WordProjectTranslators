using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models.Storages.Models;
using Translators.ServiceManagers;

namespace Translators.Models.Storages
{
    public class ApplicationReadingData : ApplicationStorageBase<ReadingData>
    {
        public ApplicationReadingData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationReadingData.json");
        }

        public static ApplicationReadingData Current { get; set; } = new ApplicationReadingData();

        public static bool IsSwitchingToNewReading { get; set; }
        public static PageData CurrentReadingData { get; set; }
        public void AddPageValue(PageType pageType, long pageNumber, long bookId, long dataId)
        {
            if (CurrentReadingData == null)
                return;
            var newValue = new PageValue()
            {
                Id = pageNumber,
                ParentId = bookId,
                PageType = pageType,
                DataId = dataId
            };
            var find = CurrentReadingData.Pages.FirstOrDefault(x => x.GetKey() == newValue.GetKey());
            if (find != null)
            {
                CurrentReadingData.Pages.Remove(find);
            }

            CurrentReadingData.Pages.Add(newValue);
            if (pageType == PageType.Pages)
            {
                _ = SaveFile();
                _ = SyncReading();
            }
        }

        public void Add(string name)
        {
            Value.Items.Add(new PageData()
            {
                Name = name
            });
            _ = SaveFile();
        }

        public void Remove(PageData pageData)
        {
            if (CurrentReadingData == pageData)
                CurrentReadingData = null;
            Value.Items.Remove(pageData);
            _ = SaveFile();
        }

        public static void SetTitle(string title)
        {
            if (CurrentReadingData == null)
                return;
            CurrentReadingData.Title = title;
            _ = Current.SaveFile();
        }


        internal static async Task SyncReading()
        {
            var userReadings = await TranslatorService.GetUserReadingService(true).GetUserReadingsAsync();
            if (userReadings.IsSuccess)
            {
                // fetch readings from live
                foreach (var reading in userReadings.Result)
                {
                    if (ApplicationReadingData.Current.Value.Items.Any(x => x.Name == reading.Name))
                        continue;
                    ApplicationReadingData.Current.Value.Items.Add(new PageData()
                    {
                        Name = reading.Name,
                        Title = reading.Title,
                        Pages = new System.Collections.Generic.List<PageValue>()
                        {
                             new PageValue()
                             {
                                  PageType = PageType.Book,
                                  Id = reading.CategoryId
                             },
                             new PageValue()
                             {
                                  PageType = PageType.Chapter,
                                  Id = reading.CatalogId,
                                  DataId = reading.CatalogId,
                                  ParentId = reading.CategoryId
                             },

                             new PageValue()
                             {
                                  PageType = PageType.Pages,
                                  Id = reading.StartPageNumber,
                                  ParentId = reading.BookId,
                                  DataId = reading.CatalogId,
                             }
                        }
                    });
                }
            }
            var readingQuery = ApplicationReadingData.Current.Value.Items.Where(x => x.Pages.Count >= 3).ToList();
            //sync readings to live
            if (readingQuery.Count == 0)
                return;
            var updateReadings = await TranslatorService.GetUserReadingService(true).SyncUserReadingsAsync(readingQuery.Select(x => new TranslatorApp.GeneratedServices.UserReadingContract()
            {
                BookId = x.Pages.FirstOrDefault(x => x.PageType == PageType.Pages).ParentId,
                CatalogId = x.Pages.FirstOrDefault(x => x.PageType == PageType.Chapter).Id,
                CategoryId = x.Pages.FirstOrDefault(x => x.PageType == PageType.Book).Id,
                PageId = x.Pages.FirstOrDefault(x => x.PageType == PageType.Pages).Id,
                StartPageNumber = (int)x.Pages.FirstOrDefault(x => x.PageType == PageType.Pages).Id,
                Name = x.Name,
                Title = x.Title
            }).ToList());
        }
    }
}