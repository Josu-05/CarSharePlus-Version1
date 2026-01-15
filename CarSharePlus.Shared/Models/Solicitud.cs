using System;
using System.ComponentModel.DataAnnotations;

namespace CarSharePlus.Shared.Models
{
    public class Solicitud
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "El tipo de solicitud es obligatorio")]
        public TipoSolicitud Tipo { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }

    public enum TipoSolicitud
    {
        Edicion,
        Eliminacion
    }
}
