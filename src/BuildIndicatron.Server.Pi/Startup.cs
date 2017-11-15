using System.Reflection;
using System.Web.Http.Dependencies;
using BuildIndicatron.Server.Pi.AppStartup;
using BuildIndicatron.Server.Pi.Swagger;
using BuildIndicatron.Server.Pi.WebApi;
using log4net;
using Owin;

namespace BuildIndicatron.Server.Pi
{
    public class Startup
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder appBuilder)
        {
            _log.Debug("Startup:Configuration start");
            CrossOrginSetup.UseCors(appBuilder);
            var webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
            SwaggerSetup.Initialize(webApiSetup.Configuration);
            SimpleFileServer.Initialize(appBuilder);
            webApiSetup.Configuration.EnsureInitialized();
            _log.Debug("Startup:Configuration done");
        }
    }
}