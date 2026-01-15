using CarSharePlus.Data;
using CarSharePlus.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharePlus.Controllers
{
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Reserva)
                .ToListAsync();
            return View(pagos);
        }

        // GET: Pagos/Create
        public IActionResult Create(int reservaId)
        {
            var pago = new Pago
            {
                ReservaId = reservaId,
                FechaPago = DateTime.Now,
                Confirmado = false
            };
            return View(pago);
        }

        // POST: Pagos/Create
        [HttpPost]
        public async Task<IActionResult> Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                if (pago.Monto <= 0)
                {
                    ModelState.AddModelError("Monto", "El monto debe ser mayor a 0.");
                    return View(pago);
                }

                var reserva = await _context.Reservas.FindAsync(pago.ReservaId);
                if (reserva == null)
                {
                    ModelState.AddModelError("ReservaId", "La reserva asociada no existe.");
                    return View(pago);
                }

                var existePagoConfirmado = _context.Pagos.Any(p => p.ReservaId == pago.ReservaId && p.Confirmado);
                if (existePagoConfirmado)
                {
                    ModelState.AddModelError("", "Ya existe un pago confirmado para esta reserva.");
                    return View(pago);
                }

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pago registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();
            return View(pago);
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Pago pago)
        {
            if (ModelState.IsValid)
            {
                if (pago.Monto <= 0)
                {
                    ModelState.AddModelError("Monto", "El monto debe ser mayor a 0.");
                    return View(pago);
                }

                _context.Update(pago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pago actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null) return NotFound();
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                if (pago.Confirmado)
                {
                    TempData["ErrorMessage"] = "No se puede eliminar un pago confirmado.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pago eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pago == null) return NotFound();
            return View(pago);
        }

        // Confirmar pago
        public async Task<IActionResult> Confirmar(int id)
        {   
        var pago = await _context.Pagos.FindAsync(id);
        if (pago == null) return NotFound();

        if (pago.Confirmado)
        {
            TempData["ErrorMessage"] = "El pago ya estaba confirmado.";
            return RedirectToAction(nameof(Index));
        }

        pago.Confirmado = true;
        _context.Update(pago);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Pago confirmado correctamente.";
        }
        catch (DbUpdateConcurrencyException)
        {
            TempData["ErrorMessage"] = "Error al confirmar el pago. Intente nuevamente.";
        }

        return RedirectToAction(nameof(Index));
        }

        }
}
