using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translators.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translators.UI.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();
            NavigationManager.InitializeSearchNavigation(Navigation);
		}
	}
}