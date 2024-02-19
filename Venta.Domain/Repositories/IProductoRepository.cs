using Venta.Domain.Models;

namespace Venta.Domain.Repositories
{
    public interface IProductoRepository : IRepository
    {
        Task<bool> Adicionar(Producto entity);
        Task<bool> Modificar(Producto entity);
        Task<bool> Eliminar(Producto entity);
        Task<Producto> Consultar(int idProducto);
        Task<IEnumerable<Producto>> Consultar(string nombre);

    }
}
