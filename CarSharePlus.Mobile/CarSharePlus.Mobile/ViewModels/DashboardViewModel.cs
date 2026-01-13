using CarSharePlus.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;

namespace CarSharePlus.Mobile.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public int TotalReservas { get; }
        public int Activas { get; }
        public int Finalizadas { get; }
        public int Canceladas { get; }
        public double PromedioDuracionHoras { get; }

        public Chart Chart { get; }

        public DashboardViewModel(EstadisticasService estadisticas)
        {
            TotalReservas = estadisticas.TotalReservas;
            Activas = estadisticas.Activas;
            Finalizadas = estadisticas.Finalizadas;
            Canceladas = estadisticas.Canceladas;
            PromedioDuracionHoras = estadisticas.PromedioDuracionHoras;

            Chart = new PieChart
            {
                Entries = new[]
                {
                    new ChartEntry(Activas) { Label = "Activas", ValueLabel = Activas.ToString(), Color = SKColor.Parse("#00FF00") },
                    new ChartEntry(Finalizadas) { Label = "Finalizadas", ValueLabel = Finalizadas.ToString(), Color = SKColor.Parse("#0000FF") },
                    new ChartEntry(Canceladas) { Label = "Canceladas", ValueLabel = Canceladas.ToString(), Color = SKColor.Parse("#FF0000") }
                }
            };
        }
    }
}
