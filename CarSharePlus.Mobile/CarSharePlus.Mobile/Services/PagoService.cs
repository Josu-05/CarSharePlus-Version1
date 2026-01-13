using CarSharePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CarSharePlus.Mobile.Services
{
    public class PagoService
    {
        public ObservableCollection<Pago> Pagos { get; } = new();

        public void RegistrarPago(Pago pago)
        {
            pago.Id = Pagos.Count + 1;
            pago.Confirmado = true;
            pago.FechaPago = DateTime.Now;
            Pagos.Add(pago);
        }
    }

}
