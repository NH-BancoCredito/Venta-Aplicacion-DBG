using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.CircuitBreaker;
using System.Reflection;
using System.Runtime.CompilerServices;
using Venta.CrossCutting.Configs;
using Venta.Domain.Repositories;
using Venta.Domain.WebServices;
using Venta.Infraestructure.Repositories;
using Venta.Infraestructure.Repositories.Base;
using Venta.Infraestructure.WebServices;
using Serilog;

namespace Venta.Infraestructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, IConfiguration configuration
            )
        {
            var appConfiguration = new AppConfiguration(configuration);
            var servicioStock = appConfiguration.UrlBaseServicioStock;
            var servicioPago = appConfiguration.UrlBaseServicioPago;

            var httpClientBuilder = services.AddHttpClient<IStocksService, StocksService>(
                options =>
                {
                    options.BaseAddress = new Uri(servicioStock);
                    //options.Timeout = TimeSpan.FromMilliseconds(5000);
                }
            ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            var httpClientPago = services.AddHttpClient<IPagoService, PagoService>(
               options =>
               {
                   options.BaseAddress = new Uri(servicioPago);
               }
           ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
           .AddPolicyHandler(GetRetryPolicy())
           .AddPolicyHandler(GetCircuitBreakerPolicy());

            var connectionString = appConfiguration.ConexionDBVentas;
            //var connectionString = new AppConfiguration(configuration).ConexionDBVentas();

            services.AddDbContext<VentaDbContext>(
                options => {
                    options.UseSqlServer(connectionString);
                    options.EnableSensitiveDataLogging();
                    }
            );
            services.AddRepositories(Assembly.GetExecutingAssembly());

            services.AddLogger(appConfiguration.LogMongoServerDB, appConfiguration.LogMongoDBCollection);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2, retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                //Camino altenativo para llamar a otro servicio failure o publicar en una cola(topico kafka)
                Console.WriteLine(result);
            };

            Action onReset = null;

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(c => !c.IsSuccessStatusCode)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30), 
                onBreak, onReset
                );
        }

        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var respositoryTypes = assembly
                .GetExportedTypes().Where(item => item.GetInterface(nameof(IRepository)) != null).ToList();


            foreach (var repositoryType in respositoryTypes)
            {
                var repositoryInterfaceType = repositoryType.GetInterfaces()
                    .Where(item => item.GetInterface(nameof(IRepository)) != null)
                    .First();

                services.AddScoped(repositoryInterfaceType, repositoryType);
            }

        }

        public static void AddLogger(this IServiceCollection services, string connectionStringDbLog, string collectionName)
        {
            var serilogLogger = new LoggerConfiguration()
                //.MinimumLevel.Error()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.MongoDB(connectionStringDbLog, collectionName: collectionName)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }

        //private static void SetHttpClient<TClient, TImplementation>(this IServiceCollection services, string constante) where TClient : class where TImplementation : class, TClient
        //{
        //    services.AddHttpClient<TClient, TImplementation>(options =>
        //    {
        //        options.Timeout = TimeSpan.FromMilliseconds(2000);
        //    })
        //        .SetHandlerLifetime(TimeSpan.FromMinutes(30))
        //        .ConfigurePrimaryHttpMessageHandler(() =>
        //        {
        //            var handler = new HttpClientHandler();
        //            //if (EnvironmentVariableProvider.IsDevelopment())
        //            //{
        //            //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        //            //}
        //            return handler;
        //        });
        //}
    }
}
