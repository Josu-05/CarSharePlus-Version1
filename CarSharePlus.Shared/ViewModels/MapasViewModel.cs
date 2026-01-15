using CarSharePlus.Shared.Models;
using CarSharePlus.Shared.Services;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace CarSharePlus.Shared.ViewModels
{
    public class MapasViewModel : BaseViewModel
    {
        private readonly OverpassService _overpassService;

        public ObservableCollection<Pin> Pins { get; } = new();

        public MapasViewModel(OverpassService overpassService)
        {
            _overpassService = overpassService;
        }

        public async Task CargarMapaAsync()
        {
            try
            {
                // Vehículos de prueba
                var vehiculos = new List<Vehiculo>
                {
                    new Vehiculo { Placa = "ABC123", Latitud = -0.1807, Longitud = -78.4678 },
                    new Vehiculo { Placa = "XYZ789", Latitud = -0.1850, Longitud = -78.4800 }
                };

                foreach (var v in vehiculos)
                {
                    Pins.Add(new Pin
                    {
                        Label = $"🚗 Vehículo {v.Placa}",
                        Location = new Microsoft.Maui.Devices.Sensors.Location(v.Latitud, v.Longitud),
                        Type = Microsoft.Maui.Controls.Maps.PinType.Place
                    });
                }

                // Ubicación del usuario
                var ubicacion = await Geolocation.GetLastKnownLocationAsync()
                               ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                if (ubicacion != null)
                {
                    Pins.Add(new Pin
                    {
                        Label = "📍 Tu ubicación",
                        Location = new Microsoft.Maui.Devices.Sensors.Location(ubicacion.Latitude, ubicacion.Longitude),
                        Type = Microsoft.Maui.Controls.Maps.PinType.SavedPin
                    });

                    // Gasolineras
                    var gasolineras = await _overpassService.BuscarLugaresAsync(ubicacion.Latitude, ubicacion.Longitude, "fuel");
                    foreach (var lugar in gasolineras)
                    {
                        Pins.Add(new Pin
                        {
                            Label = "⛽ Gasolinera",
                            Location = lugar,
                            Type = Microsoft.Maui.Controls.Maps.PinType.Place
                        });
                    }

                    // Electrolineras
                    var electrolineras = await _overpassService.BuscarLugaresAsync(ubicacion.Latitude, ubicacion.Longitude, "charging_station");
                    foreach (var lugar in electrolineras)
                    {
                        Pins.Add(new Pin
                        {
                            Label = "🔌 Electrolinera",
                            Location = lugar,
                            Type = Microsoft.Maui.Controls.Maps.PinType.Place
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cargar el mapa: {ex.Message}", "OK");
            }
        }
    }
}