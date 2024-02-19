using AutoMapper;
using NSubstitute;
using System.Collections.Generic;
using Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos;
using Venta.Domain.Models;
using Venta.Domain.Repositories;

namespace Venta.Test.Aplication.Text
{
    public class AdministrarProductosTests
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;
        private readonly ConsultarProductosHandler _consultarProductosHandler;

        public AdministrarProductosTests()
        {
            _mapper = new MapperConfiguration(c => c.AddProfile<ConsultarProductosMapper>()).CreateMapper();
            _productoRepository = Substitute.For<IProductoRepository>();
            _consultarProductosHandler = Substitute.For<ConsultarProductosHandler>(_productoRepository,_mapper);
        }


        [Fact]
        public async Task ConsultarProductos()
        {
            var request = new ConsultarProductosRequest() { filtroPorNombre="123" };
            var cancellationToken = new CancellationToken();
            IEnumerable<Producto> productos = toList();
            _productoRepository.Consultar(default(string)).ReturnsForAnyArgs(productos);
            var response = await _consultarProductosHandler.Handle(request, cancellationToken);
            //Assert.True(response.Resultado.ToList().Count > 0);
        }

        private List<Producto> toList()
        {
            var lista = new List<Producto>();

            var producto1 = new Producto()
            {
                IdProducto = 1,
                Nombre = "Aceite",
                PrecioUnitario = 5,
                Stock = 10,
                StockMinimo = 5
            };
            var producto2 = new Producto()
            {
                IdProducto = 2,
                Nombre = "Arroz",
                PrecioUnitario = 10,
                Stock = 20,
                StockMinimo = 10
            };

            lista.Add(producto1);
            lista.Add(producto2);

            return lista;

        }

    }
}
