using System.Linq;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.DataTypes;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.Models.Storages.Models;

namespace Translators.ViewModels.Pages
{
    public class ReadingPageViewModel : BaseCollectionViewModel<PageData>
    {
        public ReadingPageViewModel()
        {
            TouchedCommand = CommandHelper.Create<PageData>(Touched);
            LongTouchedCommand = CommandHelper.Create<PageData>(LongTouched);
            AddReadingCommand = CommandHelper.Create(AddReading);
            _ = LoadData(true);
        }

        public ICommand<PageData> TouchedCommand { get; set; }
        public ICommand<PageData> LongTouchedCommand { get; set; }
        public ICommand AddReadingCommand { get; set; }

        public int ReadingFontSize
        {
            get
            {
                return FontSize - 7;
            }
        }

        public async Task Touched(PageData pageData)
        {
            var selectedType = await AlertHelper.Display<ReadingRightClickType>("عملیات", "انصراف", "مطالعه", "حذف");
            switch (selectedType)
            {
                case ReadingRightClickType.Lunch:
                    {
                        ApplicationReadingData.CurrentReadingData = pageData;
                        if (string.IsNullOrEmpty(pageData.Title))
                            await PageHelper.Clean();
                        else if (pageData.Pages.Any(x => x.PageType == Models.PageType.Pages))
                        {
                            await PageHelper.Clean();
                            await ApplicationPagesData.LoadStaticPageData(pageData);
                        }
                        break;
                    }
                case ReadingRightClickType.Delete:
                    {
                        var result = await AlertHelper.DisplayQuestion("عملیات", $"آیا می خواهید خوانش '{pageData.Name}' را حذف کنید؟");
                        if (result)
                        {
                            ApplicationReadingData.Current.Remove(pageData);
                            await LoadData(true);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private async Task AddReading()
        {
            var name = await AlertHelper.DisplayPrompt("درج خوانش", "لطفا نام خوانش مورد نظر را وارد کنید");
            if (!string.IsNullOrEmpty(name))
            {
                if (ApplicationReadingData.Current.Value.Items.Any(x => x.Name == name))
                {
                    await AlertHelper.Alert("خوانش", "خوانش با همین نام وود دارد.");
                    return;
                }
                ApplicationReadingData.Current.Add(name);
                await LoadData(true);
            }
        }

        public override async Task FetchData(bool isForce)
        {
            InitialData(ApplicationReadingData.Current.Value.Items);
        }

        private async Task LongTouched(PageData arg)
        {

        }
    }
}