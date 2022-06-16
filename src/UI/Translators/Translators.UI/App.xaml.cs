using Translators.UI.Helpers;

namespace Translators.UI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        StartUp.Initialize();
        MainPage = new AppShell();
	}
}
