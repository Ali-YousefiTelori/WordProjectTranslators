using System;
using System.Collections.Generic;
using System.Text;
using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ServiceManagers;
using Xamarin.Forms;

namespace Translators.UI.Helpers
{
    public static class StartUp
    {
        public static void Initialize()
        {
            AsyncHelper.RunOnUAction = Device.BeginInvokeOnMainThread;

            _ = ApplicationProfileData.Current.BaseInitialize();
            _ = ApplicationSettingData.Current.BaseInitialize();
            _ = ApplicationReadingData.Current.BaseInitialize();
            
            ClipboardHelper.Current = new ClipboardManager();
            AlertHelper.Current = new AlertManager();
            PageHelper.Initialize(NavigationManager.Current);
            CommandHelper.Current = new CommandManager();
            TranslatorService.Initialize();
        }
    }
}
