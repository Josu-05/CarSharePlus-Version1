using MvvmHelpers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using CarSharePlus.Shared.Models;

namespace CarSharePlus.Shared.ViewModels
{
    public class AgregarEvaluacionViewModel : BaseViewModel
    {
        private int vehiculoId;
        public int VehiculoId
        {
            get => vehiculoId;
            set => SetProperty(ref vehiculoId, value);
        }

        private int calificacion;
        public int Calificacion
        {
            get => calificacion;
            set => SetProperty(ref calificacion, value);
        }

        private string comentario = string.Empty;
        public string Comentario
        {
            get => comentario;
            set => SetProperty(ref comentario, value);
        }

        public ICommand GuardarCommand { get; }

        public AgregarEvaluacionViewModel()
        {
            GuardarCommand = new Command(async () => await GuardarEvaluacion());
        }

        private async Task GuardarEvaluacion()
        {
            if (Calificacion < 1 || Calificacion > 5)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La calificación debe estar entre 1 y 5.", "OK");
                return;
            }

            var evaluacion = new EvaluacionDto
            {
                UsuarioId = 1, // Simulado, reemplazar con usuario real
                VehiculoId = VehiculoId,
                Calificacion = Calificacion,
                Comentario = Comentario,
                Fecha = DateTime.Now
            };

            var json = JsonSerializer.Serialize(evaluacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync("https://tuservidor/api/evaluaciones", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Evaluación guardada correctamente.", "OK");
                await Shell.Current.GoToAsync(".."); // ✅ volver a la lista
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar la evaluación.", "OK");
            }
        }
    }
}
