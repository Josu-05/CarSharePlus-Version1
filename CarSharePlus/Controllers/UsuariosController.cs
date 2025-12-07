using CarSharePlus.Data;
using CarSharePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarSharePlus.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context) => _context = context;

        // GET: Usuarios (con búsqueda)
        public async Task<IActionResult> Index(string searchString)
        {
            var usuarios = _context.Usuarios
                .Include(u => u.Vehiculos)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(u => u.Nombre.Contains(searchString)
                                            || u.Correo.Contains(searchString));
            }

            return View(await usuarios.AsNoTracking().ToListAsync());
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound("Debe especificar un ID de usuario.");

            var usuario = await _context.Usuarios
                .Include(u => u.Vehiculos)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound($"No se encontró el usuario con ID {id}.");

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create() => View();

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Correo,Telefono")] Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            if (await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo))
            {
                ModelState.AddModelError("Correo", "El correo ya está registrado.");
                return View(usuario);
            }

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Usuario creado correctamente ✅";
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound("Debe especificar un ID de usuario.");

            var usuario = await _context.Usuarios
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound($"No se encontró el usuario con ID {id}.");

            // Lista de vehículos disponibles para asignar
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculos.Where(v => v.UsuarioId == null), "Id", "Placa");

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,Telefono")] Usuario usuario, int? VehiculoId)
        {
            if (id != usuario.Id) return BadRequest("El ID no coincide con el usuario a editar.");
            if (!ModelState.IsValid) return View(usuario);

            if (await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo && u.Id != usuario.Id))
            {
                ModelState.AddModelError("Correo", "El correo ya está registrado.");
                return View(usuario);
            }

            var original = await _context.Usuarios.Include(u => u.Vehiculos).FirstOrDefaultAsync(u => u.Id == id);
            if (original == null) return NotFound($"No se encontró el usuario con ID {id}.");

            try
            {
                original.Nombre = usuario.Nombre;
                original.Correo = usuario.Correo;
                original.Telefono = usuario.Telefono;

                // Asignar vehículo si se seleccionó
                if (VehiculoId.HasValue)
                {
                    var vehiculo = await _context.Vehiculos.FindAsync(VehiculoId.Value);
                    if (vehiculo != null)
                        vehiculo.UsuarioId = original.Id;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario actualizado correctamente ✏️";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Conflicto de concurrencia al actualizar el usuario.");
            }
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound("Debe especificar un ID de usuario.");

            var usuario = await _context.Usuarios
                .Include(u => u.Vehiculos)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null) return NotFound($"No se encontró el usuario con ID {id}.");

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound($"No se encontró el usuario con ID {id}.");

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario eliminado correctamente 🗑️";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar usuario: {ex.Message}");
            }
        }
    }
}