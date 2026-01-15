using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Shared.ViewModels
{
    public class EditarEvaluacionViewModel : BaseViewModel
    {
        private int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

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

        public ICommand ActualizarCommand { get; }

        public EditarEvaluacionViewModel(Evaluacion evaluacion)
        {
            Id = evaluacion.Id;
            VehiculoId = evaluacion.VehiculoId;
            Calificacion = evaluacion.Calificacion;
            Comentario = evaluacion.Comentario;

            ActualizarCommand = new Command(async () => await ActualizarEvaluacion());
        }

        private async Task ActualizarEvaluacion()
        {
            if (Calificacion < 1 || Calificacion > 5)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "La calificación debe estar entre 1 y 5.", "OK");
                return;
            }

            var evaluacion = new EvaluacionDto
            {
                Id = Id,
                UsuarioId = 1, // Simulado, reemplazar con usuario real
                VehiculoId = VehiculoId,
                Calificacion = Calificacion,
                Comentario = Comentario,
                Fecha = DateTime.Now
            };

            var json = JsonSerializer.Serialize(evaluacion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PutAsync($"https://tuservidor/api/evaluaciones/{Id}", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Evaluación actualizada correctamente.", "OK");
                await Shell.Current.GoToAsync(".."); // ✅ volver a la lista
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la evaluación.", "OK");
            }
        }
    }
}
