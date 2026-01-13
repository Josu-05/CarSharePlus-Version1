using CarSharePlus.Mobile.Services;
using CarSharePlus.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CarSharePlus.Mobile.ViewModels
{
    public partial class PagosViewModel : ObservableObject
    {
        public ObservableCollection<Pago> Pagos { get; }

        public PagosViewModel(PagoService pagoService)
        {
            Pagos = pagoService.Pagos;
        }
    }
}
