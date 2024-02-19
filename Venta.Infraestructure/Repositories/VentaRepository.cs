using Venta.Domain.Repositories;
using Venta.Infraestructure.Repositories.Base;

namespace Venta.Infraestructure.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly VentaDbContext _context;

        public VentaRepository(VentaDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Registrar(Domain.Models.Venta venta)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return venta.IdVenta > 0;
        }
    }
}
