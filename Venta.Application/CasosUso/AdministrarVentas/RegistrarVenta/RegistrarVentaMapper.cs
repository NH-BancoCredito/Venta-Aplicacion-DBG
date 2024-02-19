using AutoMapper;
using Models = Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public class RegistrarVentaMapper : Profile
    {
        public RegistrarVentaMapper()
        {
            CreateMap<RegistrarVentaRequest, Models.Venta>()
                .ForMember(dest => dest.Detalle, map => map.MapFrom(src => src.Productos));
            CreateMap<RegistrarVentaDetalleRequest, Models.VentaDetalle>();
        }
    }
}
