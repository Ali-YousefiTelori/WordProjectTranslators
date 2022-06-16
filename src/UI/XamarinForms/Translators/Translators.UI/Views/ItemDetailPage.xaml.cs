using System.ComponentModel;
using Translators.UI.ViewModels;
using Xamarin.Forms;

namespace Translators.UI.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}