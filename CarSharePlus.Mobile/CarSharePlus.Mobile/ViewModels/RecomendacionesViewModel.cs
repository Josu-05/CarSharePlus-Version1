using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CarSharePlus.Mobile.ViewModels;

public class RecomendacionesViewModel : BaseViewModel
{
    public ObservableCollection<VehiculoRecomendado> VehiculosRecomendados { get; set; } = new();

    public RecomendacionesViewModel()
    {
        CargarRecomendaciones();
    }

    private async void CargarRecomendaciones()
    {
        using var client = new HttpClient();
        var json = await client.GetStringAsync("https://tuservidor/api/dashboard/rankingvehiculos");
        var lista = JsonSerializer.Deserialize<List<VehiculoRecomendado>>(json);

        VehiculosRecomendados.Clear();
        foreach (var v in lista) VehiculosRecomendados.Add(v);
    }
}

public class VehiculoRecomendado
{
    public string Placa { get; set; } = string.Empty;
    public double Promedio { get; set; }
    public string Transmision { get; set; } = string.Empty;
    public string TipoEnergia { get; set; } = string.Empty;
    public int Autonomia { get; set; }
    public double Consumo { get; set; }
}
