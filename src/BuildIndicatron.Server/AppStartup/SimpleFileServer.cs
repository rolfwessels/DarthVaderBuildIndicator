using Microsoft.AspNetCore.Builder;

namespace CoreDocker.Api.AppStartup
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