using AutoMapper;
using MediatR;
using Venta.Application.Common;
using Venta.Domain.Models;
using Venta.Domain.Repositories;

namespace Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoHandler : IRequestHandler<ActualizarProductoRequest, IResult>
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        //Verificar si el producto existe
        //  si existe actualizar la tabla de productos
        // si no existe crear un nuevo registro

        public ActualizarProductoHandler(IProductoRepository productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<IResult> Handle(ActualizarProductoRequest request, CancellationToken cancellationToken)
        {
            IResult response = null;
            var result = false;
            try
            {
                var producto = _mapper.Map<Producto>(request);
                var datos = await _productoRepository.Consultar(request.IdProducto);
                if (datos == null)
                {
                    result = await _productoRepository.Adicionar(producto);
                }
                else
                {
                    result = await _productoRepository.Modificar(producto);
                }

                if (result)
                {
                    response = new SuccessResult<int>(producto.IdProducto);
                }
                else
                {
                    response = new FailureResult();
                }
                return response;

            }
            catch (Exception ex)
            {
                response = new FailureResult();
                return response;
            }
           
        }
    }
}
