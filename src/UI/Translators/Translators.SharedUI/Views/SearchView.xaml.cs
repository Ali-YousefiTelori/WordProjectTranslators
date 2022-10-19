using Translators.UI.Helpers;

namespace Translators.UI.Views
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