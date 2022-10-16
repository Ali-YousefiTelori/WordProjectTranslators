﻿using Translators.Helpers;
using Translators.Models.Storages;
using Translators.ViewModels.Pages;

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

        //async Task PlayCommand()
        //{
        //    var vm = BindingContext as PageViewModel;
        //    MediaElement.Source = MediaSource.FromFile(vm.SourceToPlay);
        //    MediaElement.Play();
        //}
    }
}