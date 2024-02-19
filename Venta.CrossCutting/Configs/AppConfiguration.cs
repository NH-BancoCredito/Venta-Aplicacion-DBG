using Microsoft.Extensions.Configuration;

namespace Venta.CrossCutting.Configs
{
    public class AppConfiguration
    {
        private readonly IConfiguration _configuration;
        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConexionDBVentas
        {
            get
            {
                return _configuration["dbVenta-cnx"];
            }
            private set { }
        }

        public string UrlBaseServicioStock
        {
            get
            {
                return _configuration["url-base-servicio-stock"];
            }
            private set { }
        }

        public string LogMongoServerDB
        {
            get
            {
                return _configuration["log-mongo-server-db"];
            }
            private set { }
        }

        public string LogMongoDBCollection
        {
            get
            {
                return _configuration["log-mongo-db-collection"];
            }
            private set { }
        }

        //public string ConexionDBVentas() => _configuration["dbVenta-cnx"];

        public string UrlBaseServicioPago
        {
            get
            {
                return _configuration["url-base-servicio-pago"];
            }
            private set { }
        }
    }
}
