using Microsoft.AspNetCore.Builder;

namespace BuildIndicatron.Server
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