using CarSharePlus.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarSharePlus.Controllers
{
    [Route("api/[controller]")][ApiController] 
    public class SolicitudesApiController : ControllerBase 
    { 
        private readonly ApplicationDbContext _context; 
        public SolicitudesApiController(ApplicationDbContext context) 
        { 
            _context = context; 
        } 
        [HttpPost] 
        public async Task<IActionResult> PostSolicitud([FromBody] Solicitud solicitud) 
        { 
            if (string.IsNullOrWhiteSpace(solicitud.Tipo) || string.IsNullOrWhiteSpace(solicitud.Descripcion)) 
                return BadRequest("Tipo y descripción son obligatorios."); solicitud.Fecha = DateTime.Now; 
            _context.Solicitudes.Add(solicitud); 
            await _context.SaveChangesAsync(); 
            return Ok(solicitud); 
        } 
    }
}
