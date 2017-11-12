using System.Linq;
using System.Reflection;
using BuildIndicatron.Core;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger.Model;

namespace BuildIndicatron.Server.Core.Swagger
{
    public class SwaggerSetup
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string _informationalVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        #region Private Methods

        private static string GetVersion()
        {
            _informationalVersion = _informationalVersion.Split('.').Take(1).StringJoin(".");
            var version = "v"+_informationalVersion;
            _log.Info("swagger version:"+ version);
            return version;
            ;
        }

        #endregion

        #region Instance

        public static void Setup(IServiceCollection services)
        {

            services.AddSwaggerGen(
                options => options.SingleApiVersion(new Info
                {
                    Title = "CoreDocker API v"+ _informationalVersion,
                    Version = GetVersion()
                }));  
            // todo: Rolf Add Auth response codes
        }

        internal static void AddUi(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUi();
        }

        #endregion
    }
}