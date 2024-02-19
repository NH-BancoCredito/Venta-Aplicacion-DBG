using MediatR;
using Venta.Application.Common;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public class RegistrarVentaRequest : IRequest <IResult>
    {
        public int IdCliente { get; set; }
        public int FormaPago { get; set; }
        public string? NumeroTarjeta { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? CVV { get; set; }
        public string? NombreTitular { get; set; }
        public int? NumeroCuotas { get; set; }
        public IEnumerable<RegistrarVentaDetalleRequest> Productos { get; set; }
    }

    public class RegistrarVentaDetalleRequest
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }
    }
}
