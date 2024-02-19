
using Confluent.Kafka;
using Venta.Domain.Services.Events;

namespace Venta.Infrastructure.Services.Events
{
    public class PlublisherFactory : IPlublisherFactory
    {
        private static Lazy<IProducer<string, string>> LazyKafkaConnection = null;

        public PlublisherFactory(ProducerConfig config)
        {
            LazyKafkaConnection = new Lazy<IProducer<string, string>>(() => new ProducerBuilder<string, string>(config).Build());
        }
        public IProducer<string, string> GetProducer()
            => LazyKafkaConnection.Value;
    }
}
