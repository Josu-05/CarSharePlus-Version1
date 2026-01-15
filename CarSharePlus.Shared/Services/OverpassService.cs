using CarSharePlus.Shared.Models;
using Microsoft.Maui.Devices.Sensors;
using System.Text.Json;

namespace CarSharePlus.Shared.Services
{
    public class OverpassService
    {
        private const string BaseUrl = "https://overpass-api.de/api/interpreter";
        private readonly HttpClient _httpClient;

        public OverpassService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Location>> BuscarLugaresAsync(double lat, double lng, string tipo, int radioMetros = 3000)
        {
            try
            {
                var query = $"[out:json];node[amenity={tipo}](around:{radioMetros},{lat},{lng});out;";
                var response = await _httpClient.GetStringAsync($"{BaseUrl}?data={Uri.EscapeDataString(query)}").ConfigureAwait(false);
                var json = JsonDocument.Parse(response);

                var lugares = new List<Location>();
                if (json.RootElement.TryGetProperty("elements", out var elements))
                {
                    foreach (var element in elements.EnumerateArray())
                    {
                        var latitud = element.GetProperty("lat").GetDouble();
                        var longitud = element.GetProperty("lon").GetDouble();
                        lugares.Add(new Location(latitud, longitud));
                    }
                }
                return lugares;
            }
            catch (Exception ex)
            {
                // Manejo de error: devolver lista vacía y opcionalmente loguear
                System.Diagnostics.Debug.WriteLine($"Error en OverpassService: {ex.Message}");
                return new List<Location>();
            }
        }
    }
}
