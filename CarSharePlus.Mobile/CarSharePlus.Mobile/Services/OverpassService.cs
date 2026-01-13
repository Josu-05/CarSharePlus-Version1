using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CarSharePlus.Mobile.Services
{
    internal class OverpassService
    {
        private const string BaseUrl = "https://overpass-api.de/api/interpreter";

        public async Task<List<Location>> BuscarLugaresAsync(double lat, double lng, string tipo)
        {
            var query = $"[out:json];node[amenity={tipo}](around:3000,{lat},{lng});out;";
            using var client = new HttpClient();
            var response = await client.GetStringAsync($"{BaseUrl}?data={Uri.EscapeDataString(query)}");
            var json = JsonDocument.Parse(response);

            var lugares = new List<Location>();
            foreach (var element in json.RootElement.GetProperty("elements").EnumerateArray())
            {
                var latitud = element.GetProperty("lat").GetDouble();
                var longitud = element.GetProperty("lon").GetDouble();
                lugares.Add(new Location(latitud, longitud));
            }
            return lugares;
        }
    }
}
