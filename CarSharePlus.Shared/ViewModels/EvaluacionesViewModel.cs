using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Shared.ViewModels
{
    public class EvaluacionesViewModel : BaseViewModel
    {
        private ObservableCollection<Evaluacion> evaluaciones = new();
        public ObservableCollection<Evaluacion> Evaluaciones
        {
            get => evaluaciones;
            set => SetProperty(ref evaluaciones, value);
        }

        public ICommand AgregarCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }

        public EvaluacionesViewModel()
        {
            AgregarCommand = new Command(async () => await AgregarEvaluacion());
            EditarCommand = new Command<Evaluacion>(async (e) => await EditarEvaluacion(e));
            EliminarCommand = new Command<Evaluacion>(async (e) => await SolicitarEliminacion(e));

            // Cargar evaluaciones al iniciar
            Task.Run(async () => await CargarEvaluaciones());
        }

        private async Task CargarEvaluaciones()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync("https://tuservidor/api/evaluaciones/usuario/1");
                var lista = JsonSerializer.Deserialize<List<Evaluacion>>(json) ?? new List<Evaluacion>();

                Evaluaciones.Clear();
                foreach (var e in lista) Evaluaciones.Add(e);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudieron cargar las evaluaciones: {ex.Message}", "OK");
            }
        }

        private async Task AgregarEvaluacion()
        {
            await Shell.Current.GoToAsync(nameof(AgregarEvaluacionPage));
        }

        private async Task EditarEvaluacion(Evaluacion evaluacion)
        {
            if (evaluacion == null) return;

            await Shell.Current.GoToAsync($"{nameof(EditarEvaluacionPage)}", new Dictionary<string, object>
            {
                { "evaluacion", evaluacion }
            });
        }

        private async Task SolicitarEliminacion(Evaluacion evaluacion)
        {
            if (evaluacion == null) return;

            var solicitud = new
            {
                UsuarioId = evaluacion.UsuarioId,
                Tipo = "Eliminación de Evaluación",
                Descripcion = $"Solicito eliminar la evaluación #{evaluacion.Id}"
            };

            var json = JsonSerializer.Serialize(solicitud);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync("https://tuservidor/api/solicitudes", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Solicitud de eliminación enviada.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar la solicitud de eliminación.", "OK");
            }
        }
    }
}
