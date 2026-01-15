using CarSharePlus.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CarSharePlus.Shared.Services
{
    public interface IReservaService
    {
        ObservableCollection<Reserva> Reservas { get; }
        void AgregarReserva(Reserva reserva);
        void CancelarReserva(Reserva reserva);
        void ActualizarReserva(Reserva reserva);
        void ActualizarEstadoReservas();
    }

    // Implementación básica (Mobile)
    public class ReservaService : IReservaService
    {
        public ObservableCollection<Reserva> Reservas { get; } = new();

        public void AgregarReserva(Reserva reserva)
        {
            reserva.Id = Reservas.Count + 1; // Simulación de ID
            reserva.Estado = EstadoReserva.Pendiente;
            Reservas.Add(reserva);
        }

        public void CancelarReserva(Reserva reserva)
        {
            var existente = Reservas.FirstOrDefault(r => r.Id == reserva.Id);
            if (existente != null)
            {
                existente.Estado = EstadoReserva.Cancelada;
            }
        }

        public void ActualizarReserva(Reserva reserva)
        {
            var existente = Reservas.FirstOrDefault(r => r.Id == reserva.Id);
            if (existente != null)
            {
                existente.VehiculoId = reserva.VehiculoId;
                existente.FechaInicio = reserva.FechaInicio;
                existente.FechaFin = reserva.FechaFin;
                existente.UbicacionInicio = reserva.UbicacionInicio;
                existente.UbicacionFin = reserva.UbicacionFin;
                existente.Estado = reserva.Estado;
            }
        }

        public void ActualizarEstadoReservas()
        {
            var ahora = DateTime.Now;
            foreach (var r in Reservas)
            {
                if (r.Estado == EstadoReserva.Cancelada || r.Estado == EstadoReserva.Finalizada)
                    continue;

                if (ahora < r.FechaInicio)
                    r.Estado = EstadoReserva.Pendiente;
                else if (ahora >= r.FechaInicio && ahora <= r.FechaFin)
                    r.Estado = EstadoReserva.Activa;
                else if (ahora > r.FechaFin)
                    r.Estado = EstadoReserva.Finalizada;
            }
        }
    }
}
