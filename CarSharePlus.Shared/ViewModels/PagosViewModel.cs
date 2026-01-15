using CarSharePlus.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarSharePlus.Shared.ViewModels
{
    public partial class PagosViewModel : ObservableObject
    {
        private readonly IPagoService _pagoService;

        [ObservableProperty]
        private ObservableCollection<Pago> pagos = new();

        public PagosViewModel(IPagoService pagoService)
        {
            _pagoService = pagoService;
            Pagos = _pagoService.Pagos;
        }

        [RelayCommand]
        private async Task RegistrarAsync()
        {
            // Aquí podrías abrir una página de registro o llamar directamente al servicio
            var nuevoPago = new Pago
            {
                Id = Guid.NewGuid().ToString(),
                FechaPago = DateTime.Now,
                Monto = 100, // Simulado
                Metodo = "Tarjeta"
            };

            var exito = await _pagoService.RegistrarPago(nuevoPago);

            if (exito)
            {
                Pagos.Add(nuevoPago);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Pago registrado correctamente.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar el pago.", "OK");
            }
        }
    }

    // Contrato compartido
    public interface IPagoService
    {
        ObservableCollection<Pago> Pagos { get; }

        Task<bool> RegistrarPago(Pago pago);
    }
}
