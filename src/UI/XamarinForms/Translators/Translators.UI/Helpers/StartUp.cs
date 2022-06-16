using System;
using System.Collections.Generic;
using System.Text;
using Translators.Helpers;

namespace Translators.UI.Helpers
{
    public static class StartUp
    {
        public static void Initialize()
        {
            ClipboardHelper.Current = new ClipboardManager();
            AlertHelper.Current = new AlertManager();
            PageHelper.Initialize(NavigationManager.Current);
            CommandHelper.Current = new CommandManager();
        }
    }
}
