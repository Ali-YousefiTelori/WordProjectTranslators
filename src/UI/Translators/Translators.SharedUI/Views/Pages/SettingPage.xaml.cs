using Translators.UI.Helpers;

namespace Translators.UI.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            NavigationManager.InitializeSettingNavigation(Navigation);
        }
    }
}