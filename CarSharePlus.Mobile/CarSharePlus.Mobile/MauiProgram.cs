using CarSharePlus.Mobile.Pages;
using CarSharePlus.Shared.ViewModels;
using CarSharePlus.Shared.Services;
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

        // ✅ Registro de servicios (interfaces → implementación)
        builder.Services.AddSingleton<IReservaService, ReservaService>();
        builder.Services.AddSingleton<IPagoService, PagoService>();
        builder.Services.AddSingleton<EstadisticasService>();

        // ✅ Registro de ViewModels y Pages
        builder.Services.AddTransient<ReservaViewModel>();
        builder.Services.AddTransient<ReservarVehiculoPage>();
        builder.Services.AddTransient<ReservasViewModel>();
        builder.Services.AddTransient<ReservasPage>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardPage>();

        // 🔧 Faltan otros registros (ejemplo)
        builder.Services.AddTransient<PerfilViewModel>();
        builder.Services.AddTransient<PerfilPage>();
        builder.Services.AddTransient<PagosViewModel>();
        builder.Services.AddTransient<PagosPage>();
        builder.Services.AddTransient<EvaluacionesViewModel>();
        builder.Services.AddTransient<EvaluacionesPage>();
        builder.Services.AddTransient<RecomendacionesViewModel>();
        builder.Services.AddTransient<RecomendacionesPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
