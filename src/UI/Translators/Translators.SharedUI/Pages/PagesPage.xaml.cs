using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ViewModels.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagesPage : ContentPage
    {
        public PagesPage()
        {
            InitializeComponent();
            ApplicationReadingData.IsSwitchingToNewReading = false;
        }

        protected override void OnDisappearing()
        {
            if (this.BindingContext is PageViewModel vm)
            {
                vm.Dispose();
            }
            base.OnDisappearing();
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null && !ApplicationReadingData.IsSwitchingToNewReading && !(this.BindingContext as PageViewModel).IsOutsideOfBookTab)
            {
                try
                {
                    if (ApplicationReadingData.CurrentReadingData != null)
                    {
                        if (await AlertHelper.DisplayQuestion("خوانش", "شما در حال خوانش هستید، ایا می‌خواهید خوانش را غیر فعال کنیم؟"))
                        {
                            ApplicationReadingData.CurrentReadingData = null;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}