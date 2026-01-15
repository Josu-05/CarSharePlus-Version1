using System;
using System.ComponentModel.DataAnnotations;

namespace CarSharePlus.Shared.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int Calificacion { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede superar los 500 caracteres.")]
        public string Comentario { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
