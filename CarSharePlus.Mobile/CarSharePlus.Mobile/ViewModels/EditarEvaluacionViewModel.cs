using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Mobile.ViewModels;

public class EditarEvaluacionViewModel : BaseViewModel
{
    public int Id { get; set; }
    public int VehiculoId { get; set; }
    public int Calificacion { get; set; }
    public string Comentario { get; set; } = string.Empty;

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

        var evaluacion = new
        {
            Id,
            UsuarioId = 1, // Simulado, reemplazar con usuario real
            VehiculoId,
            Calificacion,
            Comentario,
            Fecha = DateTime.Now
        };

        var json = JsonSerializer.Serialize(evaluacion);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PutAsync($"https://tuservidor/api/evaluaciones/{Id}", content);

        if (response.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "Evaluación actualizada correctamente.", "OK");
            await Shell.Current.GoToAsync(".."); // Volver a la lista
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la evaluación.", "OK");
        }
    }
}
