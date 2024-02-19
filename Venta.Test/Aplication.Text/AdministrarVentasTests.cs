using AutoMapper;
using NSubstitute;
using Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos;
using Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta;
using Venta.Domain.Models;
using Venta.Domain.Repositories;

namespace Venta.Test.Aplication.Text
{
    public class AdministrarVentasTests
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;
        private readonly RegistrarVentaHandler _registrarVentaHandler;

        public AdministrarVentasTests()
        {
            _mapper = new MapperConfiguration(c => c.AddProfile<RegistrarVentaMapper>()).CreateMapper();
            _ventaRepository = Substitute.For<IVentaRepository>();
            _productoRepository = Substitute.For<IProductoRepository>();
            _registrarVentaHandler = Substitute.For<RegistrarVentaHandler>(_ventaRepository, _productoRepository, _mapper);
        }


        [Fact]
        public async Task RegistrarVenta()
        {
            var request = listToVentaRequest();
            var producto = new Producto() { IdProducto = 1, Stock = 10, StockMinimo = 5 };
            _productoRepository.Consultar(default(int)).ReturnsForAnyArgs(producto);
            _ventaRepository.Registrar(default).ReturnsForAnyArgs(true);

            //var resultado = await _registrarVentaHandler.Registrar(request);
            //Assert.NotNull(resultado.VentaRegistrada);
            //Assert.True(resultado.VentaRegistrada);
        }

        [Fact]
        public RegistrarVentaRequest listToVentaRequest()
        {
            var listaVentaDetalleRequest = new List<RegistrarVentaDetalleRequest>();
            listaVentaDetalleRequest.Add(new RegistrarVentaDetalleRequest() { IdProducto = 1, Cantidad = 1, Precio = 10 });
            listaVentaDetalleRequest.Add(new RegistrarVentaDetalleRequest() { IdProducto = 2, Cantidad = 2, Precio = 20 });
            var registrarVentaRequest = new RegistrarVentaRequest() { IdCliente = 1, Productos = listaVentaDetalleRequest };
            return registrarVentaRequest;
        }
    }
}
