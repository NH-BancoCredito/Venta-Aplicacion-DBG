using AutoMapper;
using Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoMapper : Profile
    {
        public ActualizarProductoMapper()
        {
            CreateMap<ActualizarProductoRequest,Producto>();
        }
    }
}
