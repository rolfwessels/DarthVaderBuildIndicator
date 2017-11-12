using System;
using System.IO;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using log4net;
using log4net.Config;
using Microsoft.Owin.Hosting;

namespace BuildIndicatron.Server.Pi
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            XmlConfigurator.Configure(new FileInfo(Path.Combine(rootPath, "loggingSettings.xml")));
            var address = $"http://localhost:5000/";
            address.Dump("address");

            _log.Info($"Starting api on [{address}]");
            using (WebApp.Start<Startup>(address))
            {
                Console.Out.WriteLine("Press enter to stop");
                Console.ReadLine();
            }
        }
    }
}