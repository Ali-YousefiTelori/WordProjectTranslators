using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.Helpers;
using Translators.Models.Storages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagesPage : ContentPage
    {
        public PagesPage()
        {
            InitializeComponent();
            ApplicationReadingData.IsSwitchingToNewReading = false;
        }

        protected override async void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null && !ApplicationReadingData.IsSwitchingToNewReading)
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