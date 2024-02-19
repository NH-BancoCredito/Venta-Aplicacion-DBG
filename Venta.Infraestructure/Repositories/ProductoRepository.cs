using Microsoft.EntityFrameworkCore;
using Venta.Domain.Models;
using Venta.Domain.Repositories;
using Venta.Infraestructure.Repositories.Base;

namespace Venta.Infraestructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly VentaDbContext _context;

        public ProductoRepository(VentaDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Adicionar(Producto entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Producto> Consultar(int idProducto)
        {
            return await _context.Productos.FindAsync(idProducto);
        }

        public async Task<IEnumerable<Producto>> Consultar(string nombre)
        {
            return await _context.Productos.Include(p=>p.Categoria).ToListAsync();
        }

        public Task<bool> Eliminar(Producto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Modificar(Producto entity)
        {
            try
            {
                Producto p = await _context.Productos.FindAsync(entity.IdProducto);
                p.Stock = entity.Stock;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
