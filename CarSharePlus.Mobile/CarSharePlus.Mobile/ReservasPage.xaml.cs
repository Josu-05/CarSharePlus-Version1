using CarSharePlus.Mobile.ViewModels;

namespace CarSharePlus.Mobile.Pages
{
    public partial class ReservasPage : ContentPage
    {
        public ReservasPage(ReservasViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
