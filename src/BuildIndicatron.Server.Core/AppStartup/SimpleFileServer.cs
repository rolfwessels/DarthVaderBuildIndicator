using Microsoft.AspNetCore.Builder;

namespace BuildIndicatron.Server.Core.AppStartup
{
    public class SimpleFileServer
    {
        
        public static void Initialize(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

    }
}