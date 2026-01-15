using CarSharePlus.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarSharePlus.Shared.ViewModels
{
    public partial class ReservasViewModel : ObservableObject
    {
        private readonly IReservaService _reservaService;

        [ObservableProperty]
        private ObservableCollection<Reserva> reservas = new();

        public ReservasViewModel(IReservaService reservaService)
        {
            _reservaService = reservaService;
            _reservaService.ActualizarEstadoReservas();
            Reservas = _reservaService.Reservas;
        }

        [RelayCommand]
        private async Task CancelarReservaAsync(Reserva reserva)
        {
            if (reserva == null) return;

            try
            {
                _reservaService.CancelarReserva(reserva);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Reserva cancelada correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cancelar la reserva: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task EditarReservaAsync(Reserva reserva)
        {
            if (reserva == null) return;

            // Preparar datos en el servicio
            _reservaService.PrepararEdicion(reserva);

            // Navegar a la página de edición
            await Shell.Current.GoToAsync($"{nameof(ReservarVehiculoPage)}", new Dictionary<string, object>
            {
                { "reserva", reserva }
            });
        }
    }

    // Contrato compartido
    public interface IReservaService
    {
        ObservableCollection<Reserva> Reservas { get; }
        void ActualizarEstadoReservas();
        void CancelarReserva(Reserva reserva);
        void PrepararEdicion(Reserva reserva);
    }
}
