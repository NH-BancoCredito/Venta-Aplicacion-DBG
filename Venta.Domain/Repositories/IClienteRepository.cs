using Venta.Domain.Models;

namespace Venta.Domain.Repositories
{
    public interface IClienteRepository : IRepository
    {
        Task<Cliente> Consultar(int idCliente);
    }
}
