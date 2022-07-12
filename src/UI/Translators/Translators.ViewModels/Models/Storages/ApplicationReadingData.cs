using System.IO;
using System.Linq;
using Translators.Models.Storages.Models;

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
        public void AddPageValue(PageType pageType, long pageNumber, long bookId)
        {
            if (CurrentReadingData == null)
                return;
            var newValue = new PageValue()
            {
                Id = pageNumber,
                ParentId = bookId,
                PageType = pageType
            };
            var find = CurrentReadingData.Pages.FirstOrDefault(x => x.GetKey() == newValue.GetKey());
            if (find != null)
            {
                CurrentReadingData.Pages.Remove(find);
            }

            CurrentReadingData.Pages.Add(newValue);
            if (pageType == PageType.Pages)
                _ = SaveFile();
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
    }
}