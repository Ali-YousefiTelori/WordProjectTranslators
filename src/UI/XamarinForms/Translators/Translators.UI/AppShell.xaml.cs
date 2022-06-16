using System;
using System.Collections.Generic;
using Translators.UI.ViewModels;
using Translators.UI.Views;
using Xamarin.Forms;

namespace Translators.UI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
