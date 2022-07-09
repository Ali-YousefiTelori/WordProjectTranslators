using System;
using System.IO;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Interfaces;
using Translators.Models.Storages;

namespace Translators.ViewModels.Pages
{
    public class SettingPageViewModel : BaseViewModel
    {
        public SettingPageViewModel()
        {
            RemoveCacheCommand = CommandHelper.Create(RemoveCache);
        }

        public ICommand RemoveCacheCommand { get; set; }

        private async Task RemoveCache()
        {
            try
            {
                if (await AlertHelper.DisplayQuestion("حذف", "آیا می‌خواهید داده های کش حذف شوند؟"))
                {
                    Directory.Delete(ApplicationBookData.GetCurrentFolderPath(), true);
                    await AlertHelper.Alert("حذف", "داده های کش با موفقیت حذف شدند");
                }
            }
            catch (Exception ex)
            {
                await AlertExcepption(ex);
            }
        }
    }
}
