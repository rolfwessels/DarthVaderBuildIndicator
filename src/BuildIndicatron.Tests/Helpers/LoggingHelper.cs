using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace BuildIndicatron.Tests.Helpers
{
    public static class LoggingHelper
    {
        public static void InitLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("loggingSettings.xml"));
        }
    }
}