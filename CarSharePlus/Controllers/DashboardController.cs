using CarSharePlus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSharePlus.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Promedio de calificaciones por vehículo
            var promedioPorVehiculo = await _context.Evaluaciones
                .GroupBy(e => e.Vehiculo.Placa)
                .Select(g => new 
                { 
                    Vehiculo = g.Key, 
                    Promedio = g.Average(e => e.Calificacion) 
                })
                .OrderByDescending(g => g.Promedio)
                .ToListAsync();
            // Top 3 usuarios más activos
            var topUsuarios = await _context.Evaluaciones
                .GroupBy(e => e.Usuario.Nombre)
                .Select(g => 
                new 
                { 
                    Usuario = g.Key, 
                    Total = g.Count() 
                })
                .OrderByDescending(g => g.Total)
                .Take(3).ToListAsync();
            // Distribución de calificaciones
            var distribucion = await _context.Evaluaciones
                .GroupBy(e => e.Calificacion)
                .Select(g => new 
                { 
                    Calificacion = g.Key, 
                    Total = g.Count() })
                .OrderBy(g => g.Calificacion)
                .ToListAsync();
            // Ranking de vehículos recomendados (Top 5)
            var rankingVehiculos = promedioPorVehiculo.Take(5).ToList(); 
            ViewBag.PromedioPorVehiculo = promedioPorVehiculo; 
            ViewBag.TopUsuarios = topUsuarios;
            ViewBag.Distribucion = distribucion; 
            ViewBag.RankingVehiculos = rankingVehiculos; 
            return View();
        }
    }
}