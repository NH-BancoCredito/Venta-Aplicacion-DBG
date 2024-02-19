using Venta.CrossCutting.Configs;

namespace Venta.WebApi.Configurations
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddHealthCheckConfiguration 
            ( this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);

            services.AddHealthChecks()
                .AddSqlServer(connectionString: appConfiguration.ConexionDBVentas);

            return services;
        }

        public static IApplicationBuilder UseHealthCheckConfiguration ( this IApplicationBuilder builder )
        {
            builder.UseHealthChecks("/health");
            return builder;
        }
    }
}
