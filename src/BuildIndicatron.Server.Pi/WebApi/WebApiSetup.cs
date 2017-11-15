using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dependencies;
using BuildIndicatron.Server.Pi.WebApi.Filters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace BuildIndicatron.Server.Pi.WebApi
{
    public class WebApiSetup
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
        private static WebApiSetup _instance;

        protected WebApiSetup(IAppBuilder appBuilder, IDependencyResolver dependencyResolver)
        {
            var configuration = new HttpConfiguration();

            SetupRoutes(configuration);
            SetupGlobalFilters(configuration);
            SetApiCamelCase(configuration);
            CrossOrginSetup.Setup(configuration);
            appBuilder.UseWebApi(configuration);
            configuration.DependencyResolver = dependencyResolver;
            Configuration = configuration;
        }

        public HttpConfiguration Configuration { get; }

        public static WebApiSetup Instance
        {
            get
            {
                if (_instance == null) throw new Exception("Call Instance before using Intance.");
                return _instance;
            }
        }

        #region Instance

        public static WebApiSetup Initialize(IAppBuilder appBuilder, IDependencyResolver dependencyResolver)
        {
            if (_isInitialized) return _instance;
            lock (_locker)
            {
                if (!_isInitialized)
                {
                    _instance = new WebApiSetup(appBuilder, dependencyResolver);
                    _isInitialized = true;
                }
            }
            return _instance;
        }

        #endregion

        #region Private Methods

        private static void SetApiCamelCase(HttpConfiguration configuration)
        {
            var jsonFormatter = configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static void SetupRoutes(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();
        }

        private static void SetupGlobalFilters(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new CaptureExceptionFilter());
        }

        #endregion
    }
}