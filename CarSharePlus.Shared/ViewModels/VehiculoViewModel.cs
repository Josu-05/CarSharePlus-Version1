using System.ComponentModel.DataAnnotations;
using CarSharePlus.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSharePlus.Shared.ViewModels
{
    public partial class VehiculoViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50, ErrorMessage = "La marca no puede superar los 50 caracteres")]
        [ObservableProperty]
        private string marca;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50, ErrorMessage = "El modelo no puede superar los 50 caracteres")]
        [ObservableProperty]
        private string modelo;

        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        [ObservableProperty]
        private int anio;

        [Required(ErrorMessage = "La placa es obligatoria")]
        [StringLength(10, ErrorMessage = "La placa no puede superar los 10 caracteres")]
        [ObservableProperty]
        private string placa;

        [Required(ErrorMessage = "La transmisión es obligatoria")]
        [ObservableProperty]
        private TipoTransmision transmision;

        [Required(ErrorMessage = "El tipo de energía es obligatorio")]
        [ObservableProperty]
        private TipoEnergia energia;

        [ObservableProperty]
        private bool disponible;

        // 🔄 De Modelo a ViewModel 
        public static VehiculoViewModel FromModel(Vehiculo v) =>
            new VehiculoViewModel
            {
                Id = v.Id,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Placa = v.Placa,
                Transmision = v.Transmision,
                Energia = v.Energia,
                Disponible = v.Disponible
            };

        // 🔄 De ViewModel a Modelo 
        public Vehiculo ToModel() =>
            new Vehiculo
            {
                Id = Id,
                Marca = Marca,
                Modelo = Modelo,
                Anio = Anio,
                Placa = Placa,
                Transmision = Transmision,
                Energia = Energia,
                Disponible = Disponible
            };
    }
}
