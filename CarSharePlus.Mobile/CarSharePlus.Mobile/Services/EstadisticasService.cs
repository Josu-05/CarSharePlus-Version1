using CarSharePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarSharePlus.Mobile.Services
{
    public class EstadisticasService
    {
        private readonly ReservaService _reservaService;

        public EstadisticasService(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        public int TotalReservas => _reservaService.Reservas.Count;
        public int Activas => _reservaService.Reservas.Count(r => r.Estado == EstadoReserva.Activa);
        public int Finalizadas => _reservaService.Reservas.Count(r => r.Estado == EstadoReserva.Finalizada);
        public int Canceladas => _reservaService.Reservas.Count(r => r.Estado == EstadoReserva.Cancelada);

        public double PromedioDuracionHoras =>
            _reservaService.Reservas.Any()
            ? _reservaService.Reservas.Average(r => (r.FechaFin - r.FechaInicio).TotalHours)
            : 0;
    }
}
