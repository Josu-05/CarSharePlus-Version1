using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharePlus.Shared.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Edición", "Eliminación"
        public string Descripcion { get; set; } = string.Empty; 
        public DateTime Fecha { get; set; } }
    }
