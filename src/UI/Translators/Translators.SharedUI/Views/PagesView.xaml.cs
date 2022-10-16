using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ViewModels.Pages;

namespace Translators.SharedUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagesView : ContentView
    {
        public PagesView()
        {
            InitializeComponent();
            //BindingContext = (this.Content as PagesView).BindingContext;
        }
    }
}