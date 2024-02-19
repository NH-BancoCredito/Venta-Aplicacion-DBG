using Venta.Domain.Models;

namespace Venta.Domain.WebServices
{
    public interface IStocksService
    {
        Task<bool> ActualizarStock(int idProducto, int cantidad);
    }
}
