using Translators.Models.Storages;
using Translators.UI.Helpers;

namespace Translators.UI.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
            NavigationManager.Initialize(Navigation);
            _ = ApplicationPagesData.Current.BaseInitialize();
        }
    }
}