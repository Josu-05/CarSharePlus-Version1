using System;
using System.ComponentModel.DataAnnotations;

namespace CarSharePlus.Shared.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaFin { get; set; }

        [StringLength(200, ErrorMessage = "La ubicación no puede superar los 200 caracteres")]
        public string UbicacionInicio { get; set; }

        [StringLength(200, ErrorMessage = "La ubicación no puede superar los 200 caracteres")]
        public string UbicacionFin { get; set; }

        [Display(Name = "Estado de la reserva")]
        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;

        // 🔧 Propiedades calculadas para mostrar en la UI
        public string RangoFechas => $"{FechaInicio:dd/MM/yyyy HH:mm} - {FechaFin:dd/MM/yyyy HH:mm}";
        public string DescripcionVehiculo => Vehiculo != null ? $"{Vehiculo.Marca} {Vehiculo.Modelo} ({Vehiculo.Placa})" : "Vehículo no asignado";
    }

    public enum EstadoReserva
    {
        Pendiente,
        Activa,
        Finalizada,
        Cancelada
    }
}
