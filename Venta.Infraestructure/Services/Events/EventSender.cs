using Confluent.Kafka;
using Venta.Domain.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Infrastructure.Services.Events
{
    public class EventSender : IEventSender
    {
        private readonly IPlublisherFactory _plublisherProducer;

        public EventSender(IPlublisherFactory plublisherProducer)
        {
            _plublisherProducer = plublisherProducer;
        }
        public async Task PublishAsync(string topic, string serializedMessage, CancellationToken cancellationToken)
        {
            var producer = _plublisherProducer.GetProducer();
            
            await producer.ProduceAsync(
                topic.ToLower(),
                new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = serializedMessage }, 
                    cancellationToken).ConfigureAwait(false);
        }
    }
}
