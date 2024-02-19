using MediatR;
using Venta.Application.Common;

namespace Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ConsultarProductosRequest : IRequest<IResult>
    {
        public string filtroPorNombre { get; set; }
    }
}
