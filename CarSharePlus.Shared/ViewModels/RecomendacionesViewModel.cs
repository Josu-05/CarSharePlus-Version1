using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Shared.ViewModels
{
    public class RecomendacionesViewModel : BaseViewModel
    {
        private ObservableCollection<VehiculoRecomendado> vehiculosRecomendados = new();
        public ObservableCollection<VehiculoRecomendado> VehiculosRecomendados
        {
            get => vehiculosRecomendados;
            set => SetProperty(ref vehiculosRecomendados, value);
        }

        public ICommand LoadCommand { get; }

        public RecomendacionesViewModel()
        {
            LoadCommand = new Command(async () => await CargarRecomendaciones());
        }

        private async Task CargarRecomendaciones()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync("https://tuservidor/api/dashboard/rankingvehiculos");
                var lista = JsonSerializer.Deserialize<List<VehiculoRecomendado>>(json) ?? new List<VehiculoRecomendado>();

                VehiculosRecomendados.Clear();
                foreach (var v in lista) VehiculosRecomendados.Add(v);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudieron cargar las recomendaciones: {ex.Message}", "OK");
            }
        }
    }
}
