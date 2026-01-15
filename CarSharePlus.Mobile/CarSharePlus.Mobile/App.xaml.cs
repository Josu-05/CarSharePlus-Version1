namespace CarSharePlus.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Tema dinámico según sistema
            Current.UserAppTheme = AppTheme.Unspecified;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
