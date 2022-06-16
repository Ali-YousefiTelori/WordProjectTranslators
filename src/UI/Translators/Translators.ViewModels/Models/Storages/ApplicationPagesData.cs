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
            if (value?.Pages?.Count(x => x.PageType == PageType.Pages) > 0)
            {
                var book = value.Pages.FirstOrDefault(x => x.PageType == PageType.Book);
                await PageHelper.PushPage(book.Id, book.ParentId,null, PageType.Book);

                var chapter = value.Pages.FirstOrDefault(x => x.PageType == PageType.Chapter);
                await PageHelper.PushPage(chapter.Id, chapter.ParentId, null, PageType.Chapter);

                var page = value.Pages.FirstOrDefault(x => x.PageType == PageType.Pages);
                await PageHelper.PushPage(page.Id, page.ParentId, null, PageType.Pages);
            }
        }

        public void AddPageValue(PageType pageType, long pageNumber, long bookId)
        {
            var newValue = new PageValue()
            {
                Id = pageNumber,
                ParentId = bookId,
                PageType = pageType
            };
            var find = Value.Pages.FirstOrDefault(x => x.GetKey() == newValue.GetKey());
            if (find != null)
            {
                Value.Pages.Remove(find);
            }

            Value.Pages.Add(newValue);
            if (pageType == PageType.Pages)
                _ = SaveFile();
        }
    }
}