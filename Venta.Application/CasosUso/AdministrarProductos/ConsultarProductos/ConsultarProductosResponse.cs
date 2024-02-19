using Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ConsultarProductosResponse
    {
        public IEnumerable<ConsultarProducto> Resultado { get; set; }
    }

    public class ConsultarProducto
    {
        public int CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int IdCategoria { get; set; }

    }
}
