namespace Venta.Domain.WebServices
{
    public interface IPagoService
    {
        Task<bool> RegistrarPago(int idVenta, DateTime fecha, decimal monto, int formaPago, 
            string? numeroTarjeta, DateTime? fechaVencimiento, string? cvv, string? nombreTitular, int? numeroCuotas);
    }
}
