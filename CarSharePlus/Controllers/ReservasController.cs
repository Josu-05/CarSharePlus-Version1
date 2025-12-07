using CarSharePlus.Data;
using CarSharePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharePlus.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Vehiculo)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                if (reserva.FechaFin <= reserva.FechaInicio)
                {
                    ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio.");
                    return View(reserva);
                }

                var existeConflicto = _context.Reservas.Any(r =>
                    r.VehiculoId == reserva.VehiculoId &&
                    r.Estado == EstadoReserva.Activa &&
                    !(reserva.FechaFin <= r.FechaInicio || reserva.FechaInicio >= r.FechaFin)
                );

                if (existeConflicto)
                {
                    ModelState.AddModelError("", "El vehículo ya está reservado en ese rango de fechas.");
                    return View(reserva);
                }

                reserva.Estado = reserva.FechaInicio > DateTime.Now ? EstadoReserva.Pendiente :
                                 reserva.FechaFin < DateTime.Now ? EstadoReserva.Finalizada :
                                 EstadoReserva.Activa;

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                if (reserva.FechaFin <= reserva.FechaInicio)
                {
                    ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio.");
                    return View(reserva);
                }

                _context.Update(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Vehiculo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null) return NotFound();
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Vehiculo)
                .Include(r => r.Pagos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null) return NotFound();
            return View(reserva);
        }
    }
}
