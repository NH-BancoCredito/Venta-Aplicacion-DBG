
using Confluent.Kafka;

namespace Venta.Domain.Services.Events
{
    public interface IPlublisherFactory
    {
        IProducer<string, string> GetProducer();
    }
}
