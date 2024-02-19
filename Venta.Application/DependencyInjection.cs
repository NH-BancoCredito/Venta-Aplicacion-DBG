
using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Reflection;
using Venta.Application.Common;
using Venta.Domain.Services.Events;
using Venta.Infrastructure.Services.Events;

namespace Venta.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(
            this IServiceCollection services
            )
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddFluentValidation(Assembly.GetExecutingAssembly());

            services.AddMediatR( cfg => cfg.RegisterServicesFromAssemblies( Assembly.GetExecutingAssembly()) );

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>) );
            services.AddProducer();
            services.AddEventServices();
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {

            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType
                    .MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }

            return services;
        }

        public static IServiceCollection AddProducer(this IServiceCollection services)
        {
            var config = new ProducerConfig
            {
                Acks = Acks.Leader,
                BootstrapServers = "127.0.0.1:9092",
                ClientId = Dns.GetHostName(),
            };

            services.AddSingleton<IPlublisherFactory>(sp => new PlublisherFactory(config));
            return services;
        }

        private static void AddEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, EventSender>();
        }
    }
}
