using System;
using Translators.UI.Helpers;
using Translators.UI.Services;
using Translators.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            StartUp.Initialize();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
