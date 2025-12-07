using CarSharePlus.Data;
using CarSharePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarSharePlus.Controllers
{
    public class EvaluacionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EvaluacionesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var evaluaciones = _context.Evaluaciones
                .Include(e => e.Usuario)
                .Include(e => e.Vehiculo);
            return View(await evaluaciones.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Placa");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evaluacion evaluacion)
        {
            if (evaluacion.Calificacion < 1 || evaluacion.Calificacion > 5)
                ModelState.AddModelError("Calificacion", "La calificación debe estar entre 1 y 5.");
            if (ModelState.IsValid)
            {
                _context.Add(evaluacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", evaluacion.UsuarioId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Placa", evaluacion.VehiculoId);
            return View(evaluacion);
        } // Métodos Edit, Details, Delete similares al patrón CRUD }
    }
}
