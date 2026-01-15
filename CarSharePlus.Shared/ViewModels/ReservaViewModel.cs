using CarSharePlus.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarSharePlus.Shared.ViewModels
{
    public partial class ReservaViewModel : ObservableObject
    {
        private readonly IReservaService _reservaService;
        private readonly IPagoService _pagoService;

        public ReservaViewModel(IReservaService reservaService, IPagoService pagoService)
        {
            _reservaService = reservaService;
            _pagoService = pagoService;
            VehiculosDisponibles = _reservaService.ObtenerVehiculosDisponibles();
        }

        // Fecha y hora separadas para binding
        [ObservableProperty] private DateTime fechaInicioDate = DateTime.Today;
        [ObservableProperty] private TimeSpan fechaInicioTime = DateTime.Now.TimeOfDay;

        [ObservableProperty] private DateTime fechaFinDate = DateTime.Today;
        [ObservableProperty] private TimeSpan fechaFinTime = DateTime.Now.AddHours(2).TimeOfDay;

        [ObservableProperty] private Vehiculo? vehiculoSeleccionado;

        public ObservableCollection<Vehiculo> VehiculosDisponibles { get; }

        [ObservableProperty] private Reserva? reservaExistente;

        public DateTime FechaInicio => FechaInicioDate.Date + FechaInicioTime;
        public DateTime FechaFin => FechaFinDate.Date + FechaFinTime;

        public void CargarReservaExistente(Reserva reserva)
        {
            ReservaExistente = reserva;
            VehiculoSeleccionado = VehiculosDisponibles.FirstOrDefault(v => v.Id == reserva.VehiculoId);
            FechaInicioDate = reserva.FechaInicio.Date;
            FechaInicioTime = reserva.FechaInicio.TimeOfDay;
            FechaFinDate = reserva.FechaFin.Date;
            FechaFinTime = reserva.FechaFin.TimeOfDay;
        }

        [RelayCommand]
        private async Task CrearReservaAsync()
        {
            if (FechaFin <= FechaInicio)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La fecha de fin debe ser posterior a la de inicio.", "OK");
                return;
            }

            if (ReservaExistente != null)
            {
                // Actualizar reserva existente
                ReservaExistente.VehiculoId = VehiculoSeleccionado?.Id ?? 0;
                ReservaExistente.FechaInicio = FechaInicio;
                ReservaExistente.FechaFin = FechaFin;
                ReservaExistente.UbicacionInicio = "Quito";
                ReservaExistente.UbicacionFin = "Quito Norte";

                _reservaService.ActualizarReserva(ReservaExistente);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Reserva actualizada correctamente.", "OK");
            }
            else
            {
                if (VehiculoSeleccionado == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un vehículo.", "OK");
                    return;
                }

                // Crear nueva reserva
                var nuevaReserva = new Reserva
                {
                    UsuarioId = 1,
                    VehiculoId = VehiculoSeleccionado.Id,
                    FechaInicio = FechaInicio,
                    FechaFin = FechaFin,
                    UbicacionInicio = "Quito",
                    UbicacionFin = "Quito Norte"
                };

                _reservaService.AgregarReserva(nuevaReserva);

                var pago = new Pago
                {
                    ReservaId = nuevaReserva.Id,
                    Monto = 25.00m,
                    Metodo = "Tarjeta"
                };

                _pagoService.RegistrarPago(pago);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Reserva creada y pago registrado.", "OK");
            }

            await Shell.Current.GoToAsync(".."); // ✅ volver a la lista
        }
    }

    // Contratos compartidos
    public interface IReservaService
    {
        ObservableCollection<Vehiculo> ObtenerVehiculosDisponibles();
        void AgregarReserva(Reserva reserva);
        void ActualizarReserva(Reserva reserva);
    }

    public interface IPagoService
    {
        void RegistrarPago(Pago pago);
    }
}
