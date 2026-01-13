namespace CarSharePlus.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // ✅ Forzar tema claro
            Current.UserAppTheme = AppTheme.Light;

            // Si prefieres forzar tema oscuro, usa:
            // Current.UserAppTheme = AppTheme.Dark;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
