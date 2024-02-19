using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Venta.Application.Common;
using Venta.Domain.Repositories;

namespace Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos
{
    public class ConsultarProductosHandler : IRequestHandler<ConsultarProductosRequest, IResult>
    {
       
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ConsultarProductosHandler(IProductoRepository productoRepository, IMapper mapper,
            ILogger<ConsultarProductosHandler> logger)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        

        public async Task<IResult> Handle(ConsultarProductosRequest request, CancellationToken cancellationToken)
        {
            IResult response = null;
            //var response = new ConsultarProductosResponse();
            try
            {
                var datos = await _productoRepository.Consultar(request.filtroPorNombre);
                response = new SuccessResult<IEnumerable<ConsultarProducto>>(_mapper.Map<IEnumerable<ConsultarProducto>>(datos));
                //response = _mapper.Map<IEnumerable<ConsultarProducto>>(datos); -- sin result dinamico
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response = new FailureResult();
                return response;
            }
        }

    }
}
