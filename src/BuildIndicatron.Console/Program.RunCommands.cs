using System;
using System.IO;
using System.Media;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace BuildIndicatron.Console
{
    public partial class Program
    {
        private void Execute()
        {
            if (Parameters.Verbose)
            {
                AddNLogConsoleOutput();
                Log.Info(string.Format("Input: {0}", Parameters.InputFile));
                Log.Info(string.Format("Length: {0}", Parameters.MaximumLength));
                Log.Info("");
            }

            if (Parameters.MaximumLength == 1)
            {
                System.Console.Out.WriteLine("Test");
            }

            if (!string.IsNullOrEmpty(Parameters.InputFile))
            {
                if (File.Exists(Parameters.InputFile))
                {

                }
            }
        }

        private static void AddNLogConsoleOutput()
        {
            var repository = (Hierarchy) LogManager.GetRepository();
            var appender = new ConsoleAppender
                {
                    Layout = new PatternLayout("%date %-5level  [%ndc] - %message%newline")
                };
            repository.Root.AddAppender(appender);
            repository.Configured = true;
            repository.RaiseConfigurationChanged(EventArgs.Empty);
            appender.Threshold = Level.Debug;
        }
    }
}