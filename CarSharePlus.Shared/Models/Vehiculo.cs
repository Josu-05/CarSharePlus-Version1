using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> 875207af3e982a6adcdb3d4de98f46b58b45ec4a
using System.ComponentModel.DataAnnotations;

namespace CarSharePlus.Shared.Models
{
    public enum TipoTransmision
    {
        Manual,
        Automatico
    }

    public enum TipoEnergia
    {
        Gasolina,
        Electrico,
        Hibrido
    }

    [Index(nameof(Placa), IsUnique = true)]
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Z]{3}-\d{3,4}$", ErrorMessage = "Formato de placa inválido (ej: ABC-123 o ABC-1234)")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La transmisión es obligatoria")]
        public TipoTransmision Transmision { get; set; }

        [Required(ErrorMessage = "El tipo de energía es obligatorio")]
        public TipoEnergia Energia { get; set; }

        [Range(0, 2000, ErrorMessage = "La autonomía debe estar entre 0 y 2000 km")]
        public int AutonomiaKm { get; set; }

        [Range(0.1, 50, ErrorMessage = "El consumo debe estar entre 0.1 y 50 por km")]
        public double ConsumoPorKm { get; set; }

        [Display(Name = "Disponible")]
        public bool Disponible { get; set; }

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
<<<<<<< HEAD

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
=======
>>>>>>> 875207af3e982a6adcdb3d4de98f46b58b45ec4a
    }
}
