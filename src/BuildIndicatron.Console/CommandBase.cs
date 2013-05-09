using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using ManyConsole;
using NDesk.Options;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace BuildIndicatron.Console
{
    public abstract class CommandBase : ConsoleCommand
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RobotApi _robotAmi;
        private static Task<JenkensProjectsResult> _allProjects;

        protected bool Verbose { get; set; }

        protected string Message { get; set; }

        protected string Host { get; set; }

        protected bool Help { get; set; }

        protected CommandBase()
        {
            Options = new OptionSet
                {
                    {"h", "Host to connect to", s => Host = s},
                    {"m=", "additional message", s => Message = s},
                    {"v", "Print details during execution.", s => Verbose = true},
                };
        }

        


        public override int Run(string[] remainingArguments)
        {
            try
            {
                var dictionary = AppSettings.Default.StringReplaces.Split('|').Select(s => s.Split(':')).ToDictionary(kv => kv.FirstOrDefault(), kv => kv.Skip(1).FirstOrDefault());
                SequencesText2Speech.SetDefaultCleanAndReplace(dictionary);
                if (Verbose)
                {
                    AddNLogConsoleOutput();
                    Log.Info(string.Format("Host: {0}", HostApi));
                    Log.Info("");
                }
                return RunCommand(remainingArguments);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.Message);
                return 1;
            }
        }

        protected abstract int RunCommand(string[] remainingArguments);

        #region Private Methods

        protected IRobotApi BuildIndicationApi
        {
            get { return _robotAmi ?? (_robotAmi = new RobotApi(HostApi)); }
        }

        protected string HostApi
        {
            get { return Host ?? AppSettings.Default.Host; }
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


        

        #endregion

        protected void AddJenkensStatsToButton()
        {
            var allProjects = AllProjects();
            Log.Info("Downloading jenkins values");
            allProjects.Wait();
            var jenkensTextConverter = new JenkensTextConverter();
            IEnumerable<string> summaryList = jenkensTextConverter.ToSummaryList(allProjects.Result);
            Choreography[] choreography = summaryList.Select(summary => new Choreography
                {
                    Sequences = new List<Sequences>
                        {
                            new SequencesGpIo {BeginTime = 0, Pin = AppSettings.Default.LsBluePin, IsOn = true},
                            new SequencesText2Speech {BeginTime = 0, Text = summary},
                            new SequencesGpIo {BeginTime = 1000, Pin = AppSettings.Default.LsBluePin, IsOn = false},
                        }
                }).ToArray();
            Task<SetButtonChoreographyResponse> result = BuildIndicationApi.SetButtonChoreography(choreography);
            result.Wait();
        }

        public static Task<JenkensProjectsResult> AllProjects()
        {
            if (_allProjects == null)
            {
                var jenkensApi = new JenkensApi(AppSettings.Default.JenkenServer);
                _allProjects = jenkensApi.GetAllProjects();
            }
            return _allProjects;
        }

        protected static IEnumerable<SequencesGpIo> SwitchOnPin(int beginTime, int lsRedPin)
        {
            var sequencesGpIos = new[]
                {
                    new SequencesGpIo
                        {
                            BeginTime = beginTime, Pin = AppSettings.Default.LsGreenPin
                        },
                    new SequencesGpIo
                        {
                            BeginTime = beginTime, Pin = AppSettings.Default.LsBluePin
                        },
                    new SequencesGpIo
                        {
                            BeginTime = beginTime, Pin = AppSettings.Default.LsRedPin
                        },
                };
            foreach (var sequencesGpIo in sequencesGpIos)
            {
                sequencesGpIo.IsOn = sequencesGpIo.Pin == lsRedPin;
            }
            return sequencesGpIos;
        }
    }
}