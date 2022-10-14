using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.UI.Helpers
{
    public static class StartUp
    {
        public static void Initialize()
        {
            AsyncHelper.RunOnUAction = Device.BeginInvokeOnMainThread;
            AsyncHelper.RunOnUIFunc = Device.InvokeOnMainThreadAsync;
            AsyncHelper.RunOnUIResultFunc = Device.InvokeOnMainThreadAsync;

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
