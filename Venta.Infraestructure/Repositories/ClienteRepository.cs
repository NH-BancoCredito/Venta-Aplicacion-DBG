using Venta.Domain.Models;
using Venta.Domain.Repositories;
using Venta.Infraestructure.Repositories.Base;

namespace Venta.Infraestructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly VentaDbContext _ventaDbContext;

        public ClienteRepository(VentaDbContext ventaDbContext)
        {
            _ventaDbContext = ventaDbContext;
        }
        public async Task<Cliente> Consultar(int idCliente)
        {
            return await _ventaDbContext.Clientes.FindAsync(idCliente);
        }
    }
}
