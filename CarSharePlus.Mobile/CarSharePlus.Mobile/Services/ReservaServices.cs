using CarSharePlus.Shared.Models;
using System.Collections.ObjectModel;

namespace CarSharePlus.Mobile.Services
{
    public class ReservaService
    {
        public ObservableCollection<Reserva> Reservas { get; } = new();

        public void AgregarReserva(Reserva reserva)
        {
            Reservas.Add(reserva);
        }

        public void CancelarReserva(Reserva reserva)
        {
            if (Reservas.Contains(reserva))
                Reservas.Remove(reserva);
        }

        public void ActualizarReserva(Reserva reserva)
        {
            var index = Reservas.IndexOf(reserva);
            if (index >= 0)
            {
                Reservas[index] = reserva;
            }
        }

        public void ActualizarEstadoReservas()
        {
            foreach (var r in Reservas)
            {
                if (r.Estado == EstadoReserva.Cancelada || r.Estado == EstadoReserva.Finalizada)
                    continue;

                var ahora = DateTime.Now;
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
