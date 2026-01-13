using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarSharePlus.Shared.Models;
using CarSharePlus.Mobile.Services;
using System.Collections.ObjectModel;

namespace CarSharePlus.Mobile.ViewModels
{
    public partial class ReservasViewModel : ObservableObject
    {
        private readonly ReservaService _reservaService;

        public ReservasViewModel(ReservaService reservaService)
        {
            _reservaService = reservaService;
            _reservaService.ActualizarEstadoReservas();
            Reservas = _reservaService.Reservas;
        }

        public ObservableCollection<Reserva> Reservas { get; }

        [RelayCommand]
        private void CancelarReserva(Reserva reserva)
        {
            _reservaService.CancelarReserva(reserva);
        }

        [RelayCommand]
        private async Task EditarReservaAsync(Reserva reserva)
        {
            var query = new Dictionary<string, object>
            {
                { "reserva", reserva }
            };

            // ✅ Usa la ruta registrada en AppShell
            await Shell.Current.GoToAsync("ReservarVehiculoPage", query);
        }
    }
}
