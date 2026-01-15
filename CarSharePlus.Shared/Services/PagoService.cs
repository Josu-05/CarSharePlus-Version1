using CarSharePlus.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarSharePlus.Shared.Services
{
    public interface IPagoService
    {
        ObservableCollection<Pago> Pagos { get; }
        Task<bool> RegistrarPago(Pago pago);
    }

    // Implementación básica (Mobile)
    public class PagoService : IPagoService
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Pago> Pagos { get; } = new();

        public PagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegistrarPago(Pago pago)
        {
            try
            {
                // Simulación de ID y confirmación local
                pago.Id = Pagos.Count + 1;
                pago.FechaPago = DateTime.Now;

                // Enviar a la API (cuando esté lista)
                var json = JsonSerializer.Serialize(pago);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://tuservidor/api/pagos", content);

                if (response.IsSuccessStatusCode)
                {
                    pago.Confirmado = true;
                    Pagos.Add(pago);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al registrar pago: {ex.Message}");
                return false;
            }
        }
    }
}
