using Translators.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
            NavigationManager.Initialize(Navigation);
        }
    }
}