using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Venta.Domain.WebServices;

namespace Venta.Infraestructure.WebServices
{
    public class StocksService : IStocksService
    {
        private readonly HttpClient _httpClientStocks;

        public StocksService(HttpClient httpClientStocks)
        {
            _httpClientStocks = httpClientStocks;
        }
        public async Task<bool> ActualizarStock(int idProducto, int cantidad)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, "api/productos/reservar");
            var entidadSerializada = JsonSerializer.Serialize( new { IdProducto = idProducto, Cantidad = cantidad } );
            request.Content = new StringContent(entidadSerializada, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _httpClientStocks.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

    }
}
