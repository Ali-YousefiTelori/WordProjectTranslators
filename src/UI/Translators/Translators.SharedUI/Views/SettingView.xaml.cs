using Translators.UI.Helpers;

namespace Translators.SharedUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingView : ContentView
    {
        public SettingView()
        {
            InitializeComponent();
            NavigationManager.InitializeSettingNavigation(Navigation);
        }
    }
}