namespace Venta.Domain.Services.Events
{
    public interface IEventSender
    {
        Task PublishAsync(string topic, string serializedMessage, CancellationToken cancellationToken);
    }
}
