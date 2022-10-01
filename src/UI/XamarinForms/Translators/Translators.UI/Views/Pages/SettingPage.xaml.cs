
using Translators.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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