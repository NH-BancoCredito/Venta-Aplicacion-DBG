﻿namespace Venta.Domain.Models
{
    public class Entrega
    {
        public int IdVenta { get; set; }
        public DateTime Fecha
        {
            get
            {
                return DateTime.Now;
            }
            private set { }
        }
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public virtual IEnumerable<EntregaDetalle> Detalle { get; set; }

    }
}
