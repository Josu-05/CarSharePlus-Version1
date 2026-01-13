using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Mobile.ViewModels;

public class EvaluacionesViewModel : BaseViewModel
{
    public ObservableCollection<Evaluacion> Evaluaciones { get; set; } = new();

    public ICommand AgregarCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }

    public EvaluacionesViewModel()
    {
        AgregarCommand = new Command(async () => await AgregarEvaluacion());
        EditarCommand = new Command<Evaluacion>(async (e) => await EditarEvaluacion(e));
        EliminarCommand = new Command<Evaluacion>(async (e) => await SolicitarEliminacion(e));

        CargarEvaluaciones();
    }

    private async void CargarEvaluaciones()
    {
        using var client = new HttpClient();
        var json = await client.GetStringAsync("https://tuservidor/api/evaluaciones/usuario/1");
        var lista = JsonSerializer.Deserialize<List<Evaluacion>>(json);
        Evaluaciones.Clear();
        foreach (var e in lista) Evaluaciones.Add(e);
    }

    private async Task AgregarEvaluacion()
    {
        // Navegar a formulario de nueva evaluación
    }

    private async Task EditarEvaluacion(Evaluacion evaluacion)
    {
        // Navegar a formulario de edición
    }

    private async Task SolicitarEliminacion(Evaluacion evaluacion)
    {
        var solicitud = new
        {
            UsuarioId = evaluacion.UsuarioId,
            Tipo = "Eliminación de Evaluación",
            Descripcion = $"Solicito eliminar la evaluación #{evaluacion.Id}"
        };

        var json = JsonSerializer.Serialize(solicitud);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        await client.PostAsync("https://tuservidor/api/solicitudes", content);

        await Application.Current.MainPage.DisplayAlert("Solicitud enviada", "Tu solicitud ha sido registrada.", "OK");
    }
}
