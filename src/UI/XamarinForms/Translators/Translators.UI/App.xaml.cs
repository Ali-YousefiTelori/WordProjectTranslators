using System;
using System.Linq;
using System.Text;
using Translators.ServiceManagers;
using Translators.UI.Helpers;
using Translators.UI.Services;
using Translators.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI
{
    public partial class App : Application
    {

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                int.TryParse(VersionTracking.CurrentVersion, out int version);
                TranslatorService.GetHealthServiceHttp(true).AddLog(new Contracts.Common.LogContract()
                {
                     LogTrace = e.ToString(),
                     DeviceDescription = GetVersion(),
                     AppVersion = version,
                     Session = TranslatorService.Session
                });
            };
            InitializeComponent();
            StartUp.Initialize();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        static string GetVersion()
        {
            StringBuilder stringBuilder = new StringBuilder();
            // First time ever launched application
            var firstLaunch = VersionTracking.IsFirstLaunchEver;
            stringBuilder.AppendLine($"firstLaunch: {firstLaunch}");

            // First time launching current version
            var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;
            stringBuilder.AppendLine($"firstLaunchCurrent: {firstLaunchCurrent}");

            // First time launching current build
            var firstLaunchBuild = VersionTracking.IsFirstLaunchForCurrentBuild;
            stringBuilder.AppendLine($"firstLaunchBuild: {firstLaunchBuild}");

            // Current app version (2.0.0)
            var currentVersion = VersionTracking.CurrentVersion;
            stringBuilder.AppendLine($"currentVersion: {currentVersion}");

            // Current build (2)
            var currentBuild = VersionTracking.CurrentBuild;
            stringBuilder.AppendLine($"currentBuild: {currentBuild}");

            // Previous app version (1.0.0)
            var previousVersion = VersionTracking.PreviousVersion;
            stringBuilder.AppendLine($"previousVersion: {previousVersion}");

            // Previous app build (1)
            var previousBuild = VersionTracking.PreviousBuild;
            stringBuilder.AppendLine($"previousBuild: {previousBuild}");

            // First version of app installed (1.0.0)
            var firstVersion = VersionTracking.FirstInstalledVersion;
            stringBuilder.AppendLine($"firstVersion: {firstVersion}");

            // First build of app installed (1)
            var firstBuild = VersionTracking.FirstInstalledBuild;
            stringBuilder.AppendLine($"firstBuild: {firstBuild}");

            // List of versions installed (1.0.0, 2.0.0)
            var versionHistory = VersionTracking.VersionHistory;
            stringBuilder.AppendLine($"versionHistory: {string.Join("#", versionHistory.ToArray())}");

            // List of builds installed (1, 2)
            var buildHistory = VersionTracking.BuildHistory;
            stringBuilder.AppendLine($"buildHistory: {string.Join("#", buildHistory.ToArray())}");

            // Device Model (SMG-950U, iPhone10,6)
            var device = DeviceInfo.Model;
            stringBuilder.AppendLine($"device: {device}");

            // Manufacturer (Samsung)
            var manufacturer = DeviceInfo.Manufacturer;
            stringBuilder.AppendLine($"manufacturer: {manufacturer}");

            // Device Name (Motz's iPhone)
            var deviceName = DeviceInfo.Name;
            stringBuilder.AppendLine($"deviceName: {deviceName}");

            // Operating System Version Number (7.0)
            var version = DeviceInfo.VersionString;
            stringBuilder.AppendLine($"version: {version}");

            // Platform (Android)
            var platform = DeviceInfo.Platform;
            stringBuilder.AppendLine($"platform: {platform}");

            // Idiom (Phone)
            var idiom = DeviceInfo.Idiom;
            stringBuilder.AppendLine($"idiom: {idiom}");

            // Device Type (Physical)
            var deviceType = DeviceInfo.DeviceType;
            stringBuilder.AppendLine($"deviceType: {deviceType}");

            return stringBuilder.ToString();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
