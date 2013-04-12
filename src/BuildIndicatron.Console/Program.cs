using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace BuildIndicatron.Console
{
    public partial class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string LogSettingsFile = "loggingSettings.xml";
        private const int ReturnSucceeded = 0;
        private const int ReturnFailed = -1;

        private Program(ProgramParameters parameters)
        {
            Parameters = parameters;
        }

        private ProgramParameters Parameters { get; set; }

        [STAThread]
        private static int Main(string[] args)
        {
            SetupLog4Net();
            var options = new ProgramParameters();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var program = new Program(options);
                try
                {
                    program.Execute();
                }
                catch (Exception e)
                {
                    Log.Error("Program:Main " + e.Message);
                    System.Console.Out.WriteLine(e.Message);
                    return ReturnFailed;
                }
                return ReturnSucceeded;
            }
            return ReturnFailed;
        }

       
        #region Private Methods

        private static void SetupLog4Net()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(location);
            if (directoryName != null)
            {
                string log4NetFile = Path.Combine(directoryName,LogSettingsFile);
                XmlConfigurator.Configure(new FileInfo(log4NetFile));
            }
        }

        #endregion
        // ReSharper restore UnusedParameter.Local

        #region Nested type: ProgramException

        internal class ProgramException : Exception
        {
            public ProgramException()
            {
            }

            public ProgramException(string message)
                : base(message)
            {
            }

            public ProgramException(string message, Exception innerException)
                : base(message, innerException)
            {
            }
        }

        #endregion
    }
}