using CarSharePlus.Shared.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace CarSharePlus.Mobile.ViewModels
{
    public class PerfilViewModel : BaseViewModel
    {
        public Usuario Usuario { get; set; }
        public Vehiculo Vehiculo { get; set; }
        public ICommand SolicitarEdicionCommand { get; }
        public ICommand SolicitarEliminacionCommand { get; }
        public PerfilViewModel()
        { 
            Usuario = new Usuario 
            { 
                Nombre = "Josué", 
                Correo = "josue@email.com", Telefono = "0999999999" 
            }; 
            Vehiculo = new Vehiculo 
            { 
                Placa = "ABC123", 
                Transmision = "Automático", 
                Energia = "Eléctrico",
                AutonomiaKm = 300,
                ConsumoPorKm = 0 
            }; 
            SolicitarEdicionCommand = new Command(() => SolicitarEdicion()); 
            SolicitarEliminacionCommand = new Command(() => SolicitarEliminacion()); } 
        private void SolicitarEdicion() 
        { 
            Application.Current.MainPage.DisplayAlert("Solicitud", "Tu solicitud de edición ha sido enviada.", "OK"); 
        } 
        private void SolicitarEliminacion() 
        { 
            Application.Current.MainPage.DisplayAlert("Solicitud", "Tu solicitud de eliminación ha sido enviada.", "OK"); 
        }
        private async Task EnviarSolicitud(string tipo) 
        { 
            var solicitud = new 
            { 
                UsuarioId = Usuario.Id, 
                Tipo = tipo, 
                Descripcion = $"Solicitud de " +
                $"{
                    tipo.ToLower()
                    } enviada desde la app móvil." 
            }; 
            var json = JsonSerializer.Serialize(solicitud); 
            var content = new StringContent(json, Encoding.UTF8, "application/json"); 
            using var client = new HttpClient(); 
            var response = await client.PostAsync("https://tuservidor/api/solicitudes", content); 
            if (response.IsSuccessStatusCode) 
                await Application.Current.MainPage.DisplayAlert("Éxito", "Solicitud enviada correctamente.", "OK"); 
            else await Application.Current.MainPage.DisplayAlert("Error", "No se pudo enviar la solicitud.", "OK"); 
        }
        private void SolicitarEdicion() => EnviarSolicitud("Edición"); 
        private void SolicitarEliminacion() => EnviarSolicitud("Eliminación");
    }
}
