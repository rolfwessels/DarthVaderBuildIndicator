using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace BuildIndicatron.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(args.FirstOrDefault()??"http://*:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
