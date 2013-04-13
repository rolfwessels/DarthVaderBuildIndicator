using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
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
                Log.Info(string.Format("UrlToConnectTo: {0}", Parameters.UrlToConnectTo));
                Log.Info(string.Format("Input: {0}", Parameters.InputFile));
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
                    var robotApi = new RobotApi(Parameters.UrlToConnectTo);
                    Log.Info(string.Format("Checking if file is on server {0}", Parameters.InputFile));
                    var hasFileInArchive = robotApi.HasFileInArchive(Parameters.InputFile);
                    hasFileInArchive.Wait();
                    if (!HasError(hasFileInArchive))
                    {
                        if (hasFileInArchive.Result.HasFile)
                        {
                            Log.Debug("Program:Execute File found. Not uploading");
                        }
                        else
                        {
                            Log.Info(string.Format("Uploading file {0}", Parameters.InputFile));
                            var uploadFile = robotApi.UploadFile(Parameters.InputFile);
                            uploadFile.Wait();
                            Log.Info(string.Format("uploadFile.Result.FileDetails {0}", uploadFile.Result.FileDetails));
                        }
                    }
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

        private bool HasError(Task hasFileInArchive)
        {
            if (hasFileInArchive.Exception != null)
            {
                Log.Error(hasFileInArchive.Exception.Message, hasFileInArchive.Exception);
                return true;
            }
            return false;
        }
    }
}