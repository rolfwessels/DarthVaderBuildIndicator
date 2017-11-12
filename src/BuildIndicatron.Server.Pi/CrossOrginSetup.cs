using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using Owin;

namespace BuildIndicatron.Server.Pi
{
    public class CrossOrginSetup
    {
        public static void UseCors(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);
        }

        public static void Setup(HttpConfiguration configuration)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            configuration.EnableCors(cors);
        }
    }
}