using AutoMapper;
using Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ConsultarProductosMapper : Profile
    {
        public ConsultarProductosMapper()
        {
            CreateMap<Producto, ConsultarProducto>()
                .ForMember(dest => dest.CodigoProducto, opt => opt.MapFrom(src => src.IdProducto));
        }
    }
}
