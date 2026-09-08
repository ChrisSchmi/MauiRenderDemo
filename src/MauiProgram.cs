using Microsoft.Extensions.Logging;

namespace MauiRenderDemo;

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

                // Beispiel für Font Awesome 6 Free Solid:
                fonts.AddFont("fa-solid-900.otf", "FASolid");
                // Optional auch Regular/Brands:
                fonts.AddFont("fa-regular-400.otf", "FARegular");
                fonts.AddFont("fa-brands-400.otf", "FABrands");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
