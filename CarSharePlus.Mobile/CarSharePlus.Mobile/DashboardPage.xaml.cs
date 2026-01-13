using CarSharePlus.Mobile.ViewModels;

namespace CarSharePlus.Mobile.Pages;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // ✅ conecta la UI con el ViewModel
    }
}
