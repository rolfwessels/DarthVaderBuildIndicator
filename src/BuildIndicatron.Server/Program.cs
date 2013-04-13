using System;
using System.IO;
using System.Reflection;
using Nancy.Hosting.Self;
using log4net;
using log4net.Config;

namespace BuildIndicatron.Server
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string LogSettingsFile = "loggingSettings.xml";

        static void Main(string[] args)
        {
            SetupLog4Net();
            var nancyHost = new NancyHost(new Uri(Shared.ApiPaths.LocalHost));
            nancyHost.Start();
            Console.WriteLine("Nancy now listening - navigating to {0}. Press enter to stop", Shared.ApiPaths.LocalHost);
            Console.ReadKey();
            nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }


        #region Private Methods

        private static void SetupLog4Net()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(location);
            if (directoryName != null)
            {
                string log4NetFile = Path.Combine(directoryName, LogSettingsFile);
                XmlConfigurator.Configure(new FileInfo(log4NetFile));
            }
        }

        #endregion
    }
}
