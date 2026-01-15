using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;

namespace CarSharePlus.Shared.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty] private int totalReservas;
        [ObservableProperty] private int activas;
        [ObservableProperty] private int finalizadas;
        [ObservableProperty] private int canceladas;
        [ObservableProperty] private double promedioDuracionHoras;
        [ObservableProperty] private Chart chart;

        public DashboardViewModel()
        {
            // Datos simulados, luego vendrán de EstadisticasService
            TotalReservas = 20;
            Activas = 5;
            Finalizadas = 12;
            Canceladas = 3;
            PromedioDuracionHoras = 4.5;

            Chart = new PieChart
            {
                Entries = new[]
                {
                    new ChartEntry(Activas) { Label = "Activas", ValueLabel = Activas.ToString(), Color = SKColor.Parse("#4CAF50") },
                    new ChartEntry(Finalizadas) { Label = "Finalizadas", ValueLabel = Finalizadas.ToString(), Color = SKColor.Parse("#2196F3") },
                    new ChartEntry(Canceladas) { Label = "Canceladas", ValueLabel = Canceladas.ToString(), Color = SKColor.Parse("#F44336") }
                }
            };
        }
    }
}