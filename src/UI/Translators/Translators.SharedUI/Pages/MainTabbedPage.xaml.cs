using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.UI.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        public MainTabbedPage ()
        {
            InitializeComponent();
            Navigation.PushAsync(new CategoryPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
        }
    }
}