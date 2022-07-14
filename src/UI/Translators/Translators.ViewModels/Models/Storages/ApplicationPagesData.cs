using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Storages.Models;

namespace Translators.Models.Storages
{
    public class ApplicationPagesData : ApplicationStorageBase<PageData>
    {
        public ApplicationPagesData()
        {
            FilePath = Path.Combine(GetFolderPath(), "ApplicationPagesData.json");
        }

        public static ApplicationPagesData Current { get; set; } = new ApplicationPagesData();

        public override async Task Load(PageData value)
        {
            await LoadStaticPageData(value);
        }

        public static async Task LoadStaticPageData(PageData value)
        {
            if (value?.Pages?.Count(x => x.PageType == PageType.Pages) > 0)
            {
                var book = value.Pages.FirstOrDefault(x => x.PageType == PageType.Book);
                await PageHelper.PushPage(book.Id, book.ParentId, null, PageType.Book, null);

                var chapter = value.Pages.FirstOrDefault(x => x.PageType == PageType.Chapter);
                await PageHelper.PushPage(chapter.Id, chapter.ParentId, null, PageType.Chapter, null);

                var page = value.Pages.FirstOrDefault(x => x.PageType == PageType.Pages);
                await PageHelper.PushPage(page.Id, page.ParentId, page.DataId, PageType.Pages, null);
            }
        }

        public void AddPageValue(PageType pageType, long pageNumber, long bookId, long dataId)
        {
            var newValue = new PageValue()
            {
                Id = pageNumber,
                DataId = dataId,
                ParentId = bookId,
                PageType = pageType
            };
            lock (Value)
            {
                var find = Value.Pages.FirstOrDefault(x => x.GetKey() == newValue.GetKey());
                if (find != null)
                {
                    Value.Pages.Remove(find);
                }

                Value.Pages.Add(newValue);
                if (pageType == PageType.Pages)
                    _ = SaveFile();

                ApplicationReadingData.Current.AddPageValue(pageType, pageNumber, bookId, dataId);
            }
        }
    }
}