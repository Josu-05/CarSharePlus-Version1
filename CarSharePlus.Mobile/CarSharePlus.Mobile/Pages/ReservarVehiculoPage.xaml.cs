using CarSharePlus.Mobile.ViewModels;
using CarSharePlus.Shared.Models;

namespace CarSharePlus.Mobile.Pages
{
    public partial class ReservarVehiculoPage : ContentPage
    {
        private readonly ReservaViewModel _viewModel;

        public ReservarVehiculoPage(ReservaViewModel viewModel, Reserva reserva)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;

            // Cargar la reserva directamente
            _viewModel.CargarReservaExistente(reserva);
        }
    }
}
