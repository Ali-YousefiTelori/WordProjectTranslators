using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ServiceManagers;
using Translators.ViewModels;

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
                        await BaseViewModel.AlertExcepption(ex);
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
                        await BaseViewModel.AlertExcepption(ex);
                    }
                });
                return result;
            };
#else
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
            ApplicationHelper.Current = new ApplicationManager();
            TranslatorService.Initialize();
        }
    }
}
