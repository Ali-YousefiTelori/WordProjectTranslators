using Translators.UI.Helpers;

namespace Translators.UI.Views
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