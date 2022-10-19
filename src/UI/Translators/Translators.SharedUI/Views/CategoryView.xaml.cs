using Translators.Models.Storages;
using Translators.UI.Helpers;

namespace Translators.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryView : ContentView
    {
        public CategoryView()
        {
            InitializeComponent();
            NavigationManager.Initialize(Navigation);
            _ = ApplicationPagesData.Current.BaseInitialize();
        }
    }
}