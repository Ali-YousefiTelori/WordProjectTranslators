using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ServiceManagers;

namespace Translators.UI.Helpers
{
    public static class AppStartup
    {
        public static void Initialize()
        {
            ApplicationHelper.ApplicationType = Contracts.Common.DataTypes.ApplicationType.WPF;
            StreamDownloaderBase.GetApplicationFolder = () => AppDomain.CurrentDomain.BaseDirectory;
            TranslatorService.GetCurrentVersion = () => GetVersion();
            TranslatorService.GetCurrentBuildNumber = () => GetVersionNumber();
            TranslatorService.GetVersion = GetVersion;
            TaskScheduler.UnobservedTaskException += (o, e) =>
            {
                UnhandleExceptionHappens(o, e.Exception);
            };
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                UnhandleExceptionHappens(o, (Exception)e.ExceptionObject);
            };
            StartUp.Initialize();
        }

        static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        static string GetVersionNumber()
        {
            return "2";
        }

        private static void UnhandleExceptionHappens(object sender, Exception e)
        {
            TranslatorService.LogException(e.ToString());
        }
    }
}
