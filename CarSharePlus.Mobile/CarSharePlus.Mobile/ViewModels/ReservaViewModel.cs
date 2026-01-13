using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarSharePlus.Shared.Models;
using CarSharePlus.Mobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CarSharePlus.Mobile.Pages;

namespace CarSharePlus.Mobile.ViewModels
{
    public partial class ReservaViewModel : ObservableObject
    {
        private readonly ReservaService _reservaService;
        private readonly PagoService _pagoService;

        public ReservaViewModel(ReservaService reservaService, PagoService pagoService)
        {
            _reservaService = reservaService;
            _pagoService = pagoService;
            CargarVehiculos();
        }

        [ObservableProperty]
        private DateTime fechaInicio = DateTime.Now;

        [ObservableProperty]
        private DateTime fechaFin = DateTime.Now.AddHours(2);

        [ObservableProperty]
        private Vehiculo? vehiculoSeleccionado; // ✅ Nullable

        public ObservableCollection<Vehiculo> VehiculosDisponibles { get; } = new();

        [ObservableProperty]
        private Reserva? reservaExistente;

        private void CargarVehiculos()
        {
            VehiculosDisponibles.Add(new Vehiculo { Id = 1, Placa = "ABC-123", Marca = "Toyota", Modelo = "Yaris" });
            VehiculosDisponibles.Add(new Vehiculo { Id = 2, Placa = "XYZ-789", Marca = "Chevrolet", Modelo = "Spark" });
            VehiculosDisponibles.Add(new Vehiculo { Id = 3, Placa = "DEF-456", Marca = "Kia", Modelo = "Rio" });
        }

        public void CargarReservaExistente(Reserva reserva)
        {
            ReservaExistente = reserva;
            VehiculoSeleccionado = VehiculosDisponibles.FirstOrDefault(v => v.Id == reserva.VehiculoId);
            FechaInicio = reserva.FechaInicio;
            FechaFin = reserva.FechaFin;
        }

        [RelayCommand]
        private async Task CrearReservaAsync()
        {
            if (ReservaExistente != null)
            {
                // Actualizar reserva existente
                ReservaExistente.VehiculoId = VehiculoSeleccionado?.Id ?? 0;
                ReservaExistente.FechaInicio = FechaInicio;
                ReservaExistente.FechaFin = FechaFin;
                ReservaExistente.UbicacionInicio = "Quito";
                ReservaExistente.UbicacionFin = "Quito Norte";

                _reservaService.ActualizarReserva(ReservaExistente);

                await Shell.Current.GoToAsync(nameof(ReservasPage), new Dictionary<string, object>
                {
                    ["reserva"] = ReservaExistente
                });
            }
            else
            {
                if (VehiculoSeleccionado == null)
                    return;

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

                await Shell.Current.GoToAsync(nameof(ReservasPage), new Dictionary<string, object>
                {
                    ["reserva"] = nuevaReserva
                });
            }
        }
    }
}
