using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.UI.Helpers
{
    public static class StartUp
    {
        public static void Initialize()
        {
            AudioPlayerHelper.Current = new Plugin.SimpleAudioPlayer.SimpleAudioPlayerImplementation();
            Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.GetSimpleAudioPlayer = () => AudioPlayerHelper.Current;

#if (WPF)
            AsyncHelper.RunOnUAction = Application.Current.Dispatcher.Invoke;
            AsyncHelper.RunOnUIFunc = async (obj) =>
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    try
                    {
                        await obj();
                    }
                    catch (Exception ex)
                    {
                        await ViewModels.BaseViewModel.AlertExcepption(ex);
                    }
                });
            };

            AsyncHelper.RunOnUIResultFunc = async (obj) =>
            {
                object result = default;
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    try
                    {
                        result = await obj();
                    }
                    catch (Exception ex)
                    {
                        await ViewModels.BaseViewModel.AlertExcepption(ex);
                    }
                });
                return result;
            };
#else
            if (Device.RuntimePlatform == Device.iOS)
            {
                ApplicationHelper.ApplicationType = Contracts.Common.DataTypes.ApplicationType.IOS;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                ApplicationHelper.ApplicationType = Contracts.Common.DataTypes.ApplicationType.Android;
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                ApplicationHelper.ApplicationType = Contracts.Common.DataTypes.ApplicationType.UWP;
            }
            AsyncHelper.RunOnUAction = Device.BeginInvokeOnMainThread;
            AsyncHelper.RunOnUIFunc = Device.InvokeOnMainThreadAsync;
            AsyncHelper.RunOnUIResultFunc = Device.InvokeOnMainThreadAsync;
#endif
            _ = ApplicationProfileData.Current.BaseInitialize();
            _ = ApplicationSettingData.Current.BaseInitialize();
            _ = ApplicationReadingData.Current.BaseInitialize();

            ClipboardHelper.Current = new ClipboardManager();
            AlertHelper.Current = new AlertManager();
            PageHelper.Initialize(NavigationManager.Current);
            CommandHelper.Current = new CommandManager();
            if (ApplicationHelper.Current == null)
                ApplicationHelper.Current = new ApplicationManager();
            TranslatorService.Initialize();
        }
    }
}
