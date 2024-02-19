
using AutoMapper;
using Models = Venta.Domain.Models;
using Venta.Domain.Repositories;
using MediatR;
using Venta.Application.Common;
using Venta.Domain.WebServices;
using Microsoft.Extensions.Logging;
using Venta.Domain.Models;
using System.Text.Json;
using Venta.Domain.Services.Events;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public class RegistrarVentaHandler : IRequestHandler<RegistrarVentaRequest, IResult>
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IStocksService _stocksService;
        private readonly IPagoService _pagoService;
        private readonly ILogger _logger;
        private readonly IEventSender _eventSender;

        public RegistrarVentaHandler(IVentaRepository ventaRepository, IProductoRepository productoRepository, 
            IClienteRepository clienteRepository, IStocksService stocksService, IPagoService pagoService,
            IMapper mapper, ILogger<RegistrarVentaHandler> logger, IEventSender eventSender)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _stocksService = stocksService;
            _pagoService = pagoService;
            _logger = logger;
            _eventSender = eventSender;
        }

        public async Task<IResult> Handle(RegistrarVentaRequest request, CancellationToken cancellationToken)
        {
            try
            {

                IResult response = null;
                //var response = new RegistrarVentaResponse(); // sin fluent validation

                // Fluent Validation dinamico
                //var validator = new RegistrarVentaValidator();
                //var validationResult = validator.Validate(request);

                //if(!validationResult.IsValid)
                //{
                //    return new FailureResult<DetailError>(new DetailError("00'", validationResult.ToString("/")));
                //    // Sin fluent Validation
                //    //response.Mensaje = validationResult.ToString(); 
                //    //return response;
                //}

                // Aplicando el automapper para convertir el objeto Request a venta dominio

                var venta = _mapper.Map<Models.Venta>(request);
                var entrega = new Entrega();
                var lstEntregaDetalle = new List<EntregaDetalle>();

                //======= Condiciones de validaciones de datos

                foreach (var detalle in venta.Detalle)
                {
                    // Validar si el producto existe
                    var productoEncontrado = await _productoRepository.Consultar(detalle.IdProducto);
                    if (productoEncontrado?.IdProducto <= 0)
                    {
                        throw new Exception($"Producto no encontrado, código {detalle.IdProducto}");
                    }

                    // Validar Stock del productos
                    if (productoEncontrado.Stock < detalle.Cantidad)
                    {
                        throw new Exception($"El producto {detalle.Producto.Nombre} no tiene suficiente stock, código {detalle.IdProducto}");
                    }

                    var entregaDetalle = new EntregaDetalle();
                    entregaDetalle.Producto = productoEncontrado.Nombre;
                    entregaDetalle.Cantidad = detalle.Cantidad;
                    lstEntregaDetalle.Add(entregaDetalle);


                    await _stocksService.ActualizarStock(detalle.IdProducto, detalle.Cantidad);
                }

                // Reservar stock del producto
                // Si sucedio algún error al reservar el producto, retornar exception


                // Si todo esta OK
                // Registrar la venta

                var cliente = await _clienteRepository.Consultar(venta.IdCliente);
                
                if (cliente?.IdCliente <= 0)
                {
                    throw new Exception($"Cliente no encontrado, código {cliente.IdCliente}");
                }
                await _ventaRepository.Registrar(venta);
                response = new SuccessResult<int>(venta.IdVenta);
                //response.VentaRegistrada = venta.IdVenta > 0; // sin fluent validation
                
                if (response.HasSucceeded)
                {
                    var responsePago = await _pagoService.RegistrarPago(venta.IdVenta, venta.Fecha, venta.Monto, request.FormaPago, request.NumeroTarjeta,
                        request.FechaVencimiento, request.CVV, request.NombreTitular, request.NumeroCuotas);
                    if (responsePago)
                    {
                        entrega.IdVenta = venta.IdVenta;
                        entrega.NombreCliente = cliente.Nombre + " " + cliente.Apellidos;
                        entrega.Direccion = cliente.Direccion;
                        entrega.Ciudad = cliente.Ciudad;
                        entrega.Detalle = lstEntregaDetalle;

                        await _eventSender.PublishAsync("Entregas", JsonSerializer.Serialize(entrega), cancellationToken);

                    }
                }



                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new Exception($"Ha ocurrido un error al registrar la venta {ex.Message}");
            }

        }

    }
}
