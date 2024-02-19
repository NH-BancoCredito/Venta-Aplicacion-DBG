using System.Net.Mime;
using System.Text.Json;
using System.Text;
using Venta.Domain.Models;
using Venta.Domain.WebServices;

namespace Venta.Infraestructure.WebServices
{
    public class PagoService : IPagoService
    {
        private readonly HttpClient _httpClientStocks;

        public PagoService(HttpClient httpClientStocks)
        {
            _httpClientStocks = httpClientStocks;
        }
        public async Task<bool> RegistrarPago(int idVenta, DateTime fecha, decimal monto, int formaPago, 
            string? numeroTarjeta, DateTime? fechaVencimiento, string? cvv, string? nombreTitular, int? numeroCuotas)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/pago/registrar");
            var entidadSerializada = JsonSerializer.Serialize(new { IdVenta = idVenta, Fecha = fecha, Monto = monto, FormaPago = formaPago, 
                NumeroTarjeta = numeroTarjeta, FechaVencimiento = fechaVencimiento, CVV = cvv, NombreTitular = nombreTitular, NumeroCuotas = numeroCuotas});
            request.Content = new StringContent(entidadSerializada, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _httpClientStocks.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
