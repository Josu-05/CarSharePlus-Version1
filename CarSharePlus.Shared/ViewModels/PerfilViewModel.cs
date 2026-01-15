using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Shared.ViewModels
{
    public class PerfilViewModel : BaseViewModel
    {
        private Usuario usuario;
        public Usuario Usuario
        {
            get => usuario;
            set => SetProperty(ref usuario, value);
        }

        private Vehiculo vehiculo;
        public Vehiculo Vehiculo
        {
            get => vehiculo;
            set => SetProperty(ref vehiculo, value);
        }

        public ICommand SolicitarEdicionCommand { get; }
        public ICommand SolicitarEliminacionCommand { get; }

        public PerfilViewModel()
        {
            Usuario = new Usuario
            {
                Id = 1, // Simulado
                Nombre = "Josué",
                Correo = "josue@email.com",
                Telefono = "0999999999"
            };

            Vehiculo = new Vehiculo
            {
                Placa = "ABC123",
                Transmision = "Automático",
                Energia = "Eléctrico",
                AutonomiaKm = 300,
                ConsumoPorKm = 0
            };

            SolicitarEdicionCommand = new Command(async () => await EnviarSolicitud("Edición"));
            SolicitarEliminacionCommand = new Command(async () => await EnviarSolicitud("Eliminación"));
        }

        private async Task EnviarSolicitud(string tipo)
        {
            var solicitud = new
            {
                UsuarioId = Usuario.Id,
                Tipo = tipo,
                Descripcion = $"Solicitud de {tipo.ToLower()} enviada desde la app."
            };

            var json = JsonSerializer.Serialize(solicitud);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync("https://tuservidor/api/solicitudes", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", $"Solicitud de {tipo.ToLower()} enviada correctamente.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo enviar la solicitud.", "OK");
            }
        }
    }
}
