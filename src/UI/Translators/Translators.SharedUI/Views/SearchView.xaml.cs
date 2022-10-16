using Translators.UI.Helpers;

namespace Translators.SharedUI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchView : ContentView
    {
		public SearchView()
		{
			InitializeComponent();
			NavigationManager.InitializeSearchNavigation(Navigation);
		}
	}
}