using System;
using System.Collections.Generic;
using System.Text;
using Translators.Helpers;
using Translators.Models.Storages;

namespace Translators.UI.Helpers
{
    public static class StartUp
    {
        public static void Initialize()
        {
            _ = ApplicationReadingData.Current.BaseInitialize();
            ClipboardHelper.Current = new ClipboardManager();
            AlertHelper.Current = new AlertManager();
            PageHelper.Initialize(NavigationManager.Current);
            CommandHelper.Current = new CommandManager();
        }
    }
}
