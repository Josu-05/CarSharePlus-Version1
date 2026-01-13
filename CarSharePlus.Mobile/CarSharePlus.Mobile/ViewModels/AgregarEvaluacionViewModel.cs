using MvvmHelpers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Mobile.ViewModels;

public class AgregarEvaluacionViewModel : BaseViewModel
{
    public int VehiculoId { get; set; }
    public int Calificacion { get; set; }
    public string Comentario { get; set; } = string.Empty;

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

        var evaluacion = new
        {
            UsuarioId = 1, // Simulado, reemplazar con usuario real
            VehiculoId,
            Calificacion,
            Comentario,
            Fecha = DateTime.Now
        };

        var json = JsonSerializer.Serialize(evaluacion);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PostAsync("https://tuservidor/api/evaluaciones", content);

        if (response.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "Evaluación registrada correctamente.", "OK");
            await Shell.Current.GoToAsync(".."); // Volver a la lista
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar la evaluación.", "OK");
        }
    }
}
