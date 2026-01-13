namespace CarSharePlus.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Pages.ReservarVehiculoPage), typeof(Pages.ReservarVehiculoPage));
            Routing.RegisterRoute(nameof(Pages.ReservasPage), typeof(Pages.ReservasPage));        
        }
    }
}