using CarSharePlus.Mobile.Pages;
using CarSharePlus.Mobile.ViewModels;
using CarSharePlus.Mobile.Services;
using Microsoft.Extensions.Logging;

namespace CarSharePlus.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ✅ Registro de servicios y ViewModels
        builder.Services.AddSingleton<ReservaService>();
        builder.Services.AddSingleton<PagoService>();
        builder.Services.AddSingleton<EstadisticasService>();

        builder.Services.AddTransient<ReservaViewModel>();
        builder.Services.AddTransient<ReservarVehiculoPage>();
        builder.Services.AddTransient<ReservasViewModel>();
        builder.Services.AddTransient<ReservasPage>();

        // ✅ Registro del Dashboard
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
