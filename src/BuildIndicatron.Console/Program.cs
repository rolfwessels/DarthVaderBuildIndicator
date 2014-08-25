using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using ManyConsole;
using log4net.Config;

namespace BuildIndicatron.Console
{
    public partial class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string LogSettingsFile = "loggingSettings.xml";
       
        [STAThread]
        private static int Main(string[] args)
        {
            SetupLog4Net();
	        try
            {
                var commands = GetCommands();
                ConsoleCommandDispatcher.DispatchCommand(commands, args, System.Console.Out);
            }
            catch (Exception e)
            {
                _log.Error("Program:Main " + e.Message);
                System.Console.WriteLine(e);
            }
            return 0;
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
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