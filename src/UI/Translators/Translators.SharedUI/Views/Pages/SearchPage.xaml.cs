using Translators.UI.Helpers;

namespace Translators.UI.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage()
		{
			InitializeComponent();
			NavigationManager.InitializeSearchNavigation(Navigation);
		}
	}
}