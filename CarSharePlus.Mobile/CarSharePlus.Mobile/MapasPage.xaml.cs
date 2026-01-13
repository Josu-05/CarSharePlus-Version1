using CarSharePlus.Mobile.Services;
using CarSharePlus.Shared.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace CarSharePlus.Mobile;

public partial class MapasPage : ContentPage
{
    public MapasPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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
                map.Pins.Add(new Pin
                {
                    Label = $"🚗 Vehículo {v.Placa}",
                    Location = new Location(v.Latitud, v.Longitud),
                    Type = PinType.Place
                });
            }

            // Centrar mapa
            var centro = new Location(-0.1820, -78.4700);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(centro, Distance.FromKilometers(2)));

            // Ubicación del usuario
            var ubicacion = await Geolocation.GetLastKnownLocationAsync();
            if (ubicacion == null)
            {
                ubicacion = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
            }

            if (ubicacion != null)
            {
                map.Pins.Add(new Pin
                {
                    Label = "📍 Tu ubicación",
                    Location = new Location(ubicacion.Latitude, ubicacion.Longitude),
                    Type = PinType.SavedPin
                });

                var servicio = new OverpassService();

                // Gasolineras
                var gasolineras = await servicio.BuscarLugaresAsync(ubicacion.Latitude, ubicacion.Longitude, "fuel");
                foreach (var lugar in gasolineras)
                {
                    map.Pins.Add(new Pin
                    {
                        Label = "⛽ Gasolinera",
                        Location = lugar,
                        Type = PinType.Place
                    });
                }

                // Electrolineras
                var electrolineras = await servicio.BuscarLugaresAsync(ubicacion.Latitude, ubicacion.Longitude, "charging_station");
                foreach (var lugar in electrolineras)
                {
                    map.Pins.Add(new Pin
                    {
                        Label = "🔌 Electrolinera",
                        Location = lugar,
                        Type = PinType.Place
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar el mapa: {ex.Message}", "OK");
        }
    }
}
