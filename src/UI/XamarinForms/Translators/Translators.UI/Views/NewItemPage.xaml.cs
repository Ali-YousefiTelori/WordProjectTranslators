using System;
using System.Collections.Generic;
using System.ComponentModel;
using Translators.UI.Models;
using Translators.UI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}