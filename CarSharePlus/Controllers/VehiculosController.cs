using CarSharePlus.Data;
using CarSharePlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarSharePlus.Shared.Models;

namespace CarSharePlus.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehiculos
        public async Task<IActionResult> Index(
            string searchBrand,
            TipoTransmision? transmision,
            TipoEnergia? energia,
            int? anioDesde,
            int? anioHasta,
            bool? disponible)
        {
            var vehiculos = _context.Vehiculos
                .Include(v => v.Usuario) // 👈 carga el usuario asignado
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchBrand))
                vehiculos = vehiculos.Where(v => v.Marca.ToLower().Contains(searchBrand.ToLower()));

            if (transmision.HasValue)
                vehiculos = vehiculos.Where(v => v.Transmision == transmision.Value);

            if (energia.HasValue)
                vehiculos = vehiculos.Where(v => v.Energia == energia.Value);

            if (anioDesde.HasValue)
                vehiculos = vehiculos.Where(v => v.Anio >= anioDesde.Value);

            if (anioHasta.HasValue)
                vehiculos = vehiculos.Where(v => v.Anio <= anioHasta.Value);

            if (disponible.HasValue)
                vehiculos = vehiculos.Where(v => v.Disponible == disponible.Value);

            return View(await vehiculos.AsNoTracking().ToListAsync());
        }

        // GET: Vehiculos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Usuario) // 👈 carga el usuario asignado
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vehiculo == null)
                return NotFound($"No se encontró el vehículo con ID {id}");

            return View(vehiculo);
        }

        // GET: Vehiculos/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Vehiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", vehiculo.UsuarioId);
                return View(vehiculo);
            }

            bool placaExiste = await _context.Vehiculos.AnyAsync(v => v.Placa == vehiculo.Placa);
            if (placaExiste)
            {
                ModelState.AddModelError("Placa", "La placa ya está registrada.");
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", vehiculo.UsuarioId);
                return View(vehiculo);
            }

            _context.Add(vehiculo);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Vehículo creado correctamente ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehiculos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return NotFound($"No se encontró el vehículo con ID {id}");

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", vehiculo.UsuarioId);
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
                return BadRequest("El ID no coincide con el vehículo a editar");

            if (!ModelState.IsValid)
            {
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", vehiculo.UsuarioId);
                return View(vehiculo);
            }

            try
            {
                _context.Update(vehiculo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehículo actualizado correctamente ✏️";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vehiculos.Any(e => e.Id == vehiculo.Id))
                    return NotFound($"No se encontró el vehículo con ID {vehiculo.Id}");
                else
                    throw;
            }
        }

        // GET: Vehiculos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Usuario) // 👈 carga el usuario asignado
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehiculo == null)
                return NotFound($"No se encontró el vehículo con ID {id}");

            return View(vehiculo);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return NotFound($"No se encontró el vehículo con ID {id}");

            try
            {
                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vehículo eliminado correctamente 🗑️";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar vehículo: {ex.Message}");
            }
        }
    }
}
