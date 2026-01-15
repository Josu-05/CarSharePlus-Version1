using Microsoft.AspNetCore.Mvc;
using CarSharePlus.Shared.Models;
using CarSharePlus.Data;


public class VehiculosController : Controller
{
    private readonly ApplicationDbContext _context;

    public VehiculosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Vehiculos
    public async Task<IActionResult> Index(string searchBrand, TipoTransmision? transmision, TipoEnergia? energia, int? anioDesde, int? anioHasta, bool? disponible)
    {
        var vehiculos = _context.Vehiculos.Include(v => v.Usuario).AsQueryable();

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

    // GET: Vehiculos/Create
    public IActionResult Create()
    {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
        return View();
    }

    // POST: Vehiculos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehiculoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", model.UsuarioId);
            return View(model);
        }

        if (await _context.Vehiculos.AnyAsync(v => v.Placa == model.Placa))
        {
            ModelState.AddModelError("Placa", "La placa ya está registrada.");
            return View(model);
        }

        if (model.UsuarioId.HasValue && !await _context.Usuarios.AnyAsync(u => u.Id == model.UsuarioId))
        {
            ModelState.AddModelError("UsuarioId", "El usuario seleccionado no existe.");
            return View(model);
        }

        var vehiculo = new Vehiculo
        {
            Marca = model.Marca,
            Modelo = model.Modelo,
            Placa = model.Placa,
            Anio = model.Anio,
            Transmision = model.Transmision,
            Energia = model.Energia,
            Disponible = model.Disponible,
            UsuarioId = model.UsuarioId
        };

        _context.Add(vehiculo);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Vehículo creado correctamente ✅";
        return RedirectToAction(nameof(Index));
    }

    // POST: Vehiculos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VehiculoViewModel model)
    {
        if (id != model.Id) return BadRequest("El ID no coincide con el vehículo a editar");
        if (!ModelState.IsValid) return View(model);

        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo == null) return NotFound($"No se encontró el vehículo con ID {id}");

        try
        {
            vehiculo.Marca = model.Marca;
            vehiculo.Modelo = model.Modelo;
            vehiculo.Placa = model.Placa;
            vehiculo.Anio = model.Anio;
            vehiculo.Transmision = model.Transmision;
            vehiculo.Energia = model.Energia;
            vehiculo.Disponible = model.Disponible;
            vehiculo.UsuarioId = model.UsuarioId;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Vehículo actualizado correctamente ✏️";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            TempData["ErrorMessage"] = "Conflicto de concurrencia al actualizar el vehículo.";
            return View(model);
        }
    }

    // POST: Vehiculos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo == null) return NotFound($"No se encontró el vehículo con ID {id}");

        if (await _context.Reservas.AnyAsync(r => r.VehiculoId == id && r.Estado == EstadoReserva.Activa))
        {
            TempData["ErrorMessage"] = "No se puede eliminar un vehículo con reservas activas.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Vehículo eliminado correctamente 🗑️";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al eliminar vehículo: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
