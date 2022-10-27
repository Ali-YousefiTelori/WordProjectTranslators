using System.IO;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Interfaces;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.ViewModels.Pages
{
    public class SettingPageViewModel : BaseViewModel
    {
        public SettingPageViewModel()
        {
            RemoveCacheCommand = CommandHelper.Create(RemoveCache);
            DownloadOfflineCommand = CommandHelper.Create(DownloadOffline);
            PlaySpeedCommand = CommandHelper.Create(PlaySpeed);
            _ = CheckForUpdate();
        }

        public ICommand RemoveCacheCommand { get; set; }
        public ICommand DownloadOfflineCommand { get; set; }
        public ICommand PlaySpeedCommand { get; set; }

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

        public async Task CheckForUpdate()
        {
            var appVersionResult = await TranslatorService.GetApplicationService(true).GetAppVersionAsync(ApplicationHelper.ApplicationType);
            if (appVersionResult.IsSuccess)
            {
                var buildNumber = await ApplicationHelper.GetBuildNumber();
                if (buildNumber < appVersionResult.Result.ForceUpdateNumber)
                {
                    if (await AlertHelper.DisplayQuestion("بروزرسانی", "نسخه‌ی جدید نرم افزار منشتر شده است، تغییرات جدید مهم است و ممکن است در استفاده از نسخه‌ی فعلی دچار مشکل و اختلال شوید، آیا می‌خواهید دانلود کنید؟"))
                    {
                        await ApplicationHelper.Current.DownloadNewVersion();
                    }
                }
                else if (buildNumber < appVersionResult.Result.Number)
                {
                    if (await AlertHelper.DisplayQuestion("بروزرسانی", "نسخه‌ی جدید نرم افزار منشتر شده است، آیا می‌خواهید دانلود کنید؟"))
                    {
                        await ApplicationHelper.Current.DownloadNewVersion();
                    }
                }
            }
        }

        private async Task PlaySpeed()
        {
            var speed = await AlertHelper.Display("لطفا سرعت مورد نظر را انتخاب کنید", "انصراف", "1x", "1.5x", "2x");
            if (speed == "1x")
            {
                _PlaybackSpeedRato = 1;
            }
            else if (speed == "1.5x")
            {
                _PlaybackSpeedRato = 1.5;
            }
            else if (speed == "2x")
            {
                _PlaybackSpeedRato = 2;
            }
            AudioPlayerBaseHelper.CurrentBase.SetSpeed(_PlaybackSpeedRato);
            ApplicationSettingData.Current.Save();
        }
    }
}
