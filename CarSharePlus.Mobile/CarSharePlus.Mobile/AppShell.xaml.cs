namespace CarSharePlus.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar solo páginas de detalle o navegación interna
            Routing.RegisterRoute(nameof(Pages.ReservarVehiculoPage), typeof(Pages.ReservarVehiculoPage));
            Routing.RegisterRoute(nameof(Pages.ReservaDetallePage), typeof(Pages.ReservaDetallePage));
            Routing.RegisterRoute(nameof(Pages.PagoPage), typeof(Pages.PagoPage));
            Routing.RegisterRoute(nameof(Pages.EvaluacionPage), typeof(Pages.EvaluacionPage));
        }
    }
}
