using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Translators.Helpers;

namespace Translators.ViewModels.Pages
{
    public class OfflineDownloaderViewModel : BaseViewModel
    {
        public OfflineDownloaderViewModel()
        {
            OfflineDownloaderHelper.Current.DataChanged = () =>
            {
                if (OfflineDownloaderHelper.Current.IsExtracting)
                    Title = $"Extract complete {OfflineDownloaderHelper.Current.CompletedServiceCount}/{OfflineDownloaderHelper.Current.ServiceCount}";
                else
                    Title = $"Download complete {OfflineDownloaderHelper.Current.CompletedServiceCount}/{OfflineDownloaderHelper.Current.ServiceCount}";
                CompletePercent = OfflineDownloaderHelper.Current.CompletePercent;

                AsyncHelper.RunOnUI(() =>
                {
                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CompletePercent));
                });
            };

            Task.Factory.StartNew(async () =>
            {
                await Start();
            });
        }

        public string Title { get; set; }
        public double CompletePercent { get; set; }

        public async Task Start()
        {
            var result = await OfflineDownloaderHelper.Current.Start();
            
            if (result.HasValue)
            {
                await PageHelper.Current.PopSettingPage();
                if (result.Value)
                {
                    await AlertHelper.Alert("تکمیل", "عملیات با موفقیت به اتمام رسید.");
                }
                else
                {
                    await AlertHelper.Alert("خطا", "عملیات با خطا مواجه شد..");
                }
            }
        }
    }
}
