using CommunityToolkit.Maui;
using Translators.UI.Helpers;

namespace Translators.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("QuranTaha.ttf", "QuranTaha");
				fonts.AddFont("Nasim.ttf", "Nasim");
				fonts.AddFont("icomoon.ttf", "icomoon");
            });
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
		NavigationManager.GetCurrentPageFunc = () =>
		{
			return App.Current.MainPage;
		};
        return builder.Build();
	}
}
