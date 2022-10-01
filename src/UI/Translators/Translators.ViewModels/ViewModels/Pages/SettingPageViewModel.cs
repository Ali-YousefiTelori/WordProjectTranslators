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
            DownloadOfflineCommand = CommandHelper.Create(DownloadOffline);
        }

        public ICommand RemoveCacheCommand { get; set; }
        public ICommand DownloadOfflineCommand { get; set; }

        private async Task RemoveCache()
        {
            if (await AlertHelper.DisplayQuestion("حذف", "آیا می‌خواهید داده های کش حذف شوند؟"))
            {
                Directory.Delete(new ApplicationBookData().GetCurrentFolderPath(), true);
                Directory.Delete(new ApplicationBookAudioData().GetCurrentFolderPath(), true);
                OfflineDownloaderHelper.Current.DeleteCachedDownloadedFiles();
                await AlertHelper.Alert("حذف", "داده های کش با موفقیت حذف شدند");
            }
        }

        private async Task DownloadOffline()
        {
            if (await AlertHelper.DisplayQuestion("آفلاین", "آیا می‌خواهید داده های نرم افزار را به صورت آفلاین دانلود کنید؟"))
            {
                await PageHelper.PushPage(0, 0, null, Models.PageType.OfflineDownloadPage, this);
            }
        }
    }
}
