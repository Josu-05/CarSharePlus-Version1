using CarSharePlus.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarSharePlus.Controllers
{
    public class SolicitudesController : Controller 
    { 
        private readonly ApplicationDbContext _context; 
        public SolicitudesController(ApplicationDbContext context) 
        { 
            _context = context; 
        } 
        public async Task<IActionResult> Index() 
        { 
            var solicitudes = await _context.Solicitudes.Include(s => s.Usuario).ToListAsync(); 
            return View(solicitudes); 
        } 
        public async Task<IActionResult> Details(int id) 
        { 
            var solicitud = await _context.Solicitudes.Include(s => s.Usuario).FirstOrDefaultAsync(s => s.Id == id); 
            if (solicitud == null) return NotFound(); 
            return View(solicitud); 
        } 
        public async Task<IActionResult> Delete(int id) 
        { 
            var solicitud = await _context.Solicitudes.FindAsync(id); 
            if (solicitud == null) return NotFound(); 
            _context.Solicitudes.Remove(solicitud); 
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index)); 
        } 
    }
}
