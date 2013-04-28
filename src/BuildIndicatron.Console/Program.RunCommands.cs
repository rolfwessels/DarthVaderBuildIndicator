using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Models.Composition;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace BuildIndicatron.Console
{
    public partial class Program
    {
        private RobotApi _robotAmi;

        private void Execute()
        {
            if (Parameters.Verbose)
            {
                AddNLogConsoleOutput();
                Log.Info(string.Format("Host: {0}", HostApi));
                Log.Info("");
            }

            if (Parameters.SetPassive)
            {
                RunPassive();
            }
            
            if (Parameters.LightSaber)
            {
                var buildState = GetState();
                SetLightSaber(buildState, Parameters.Message);
            }

            if (Parameters.Glow)
            {
                var buildState = GetState();
                SetGlow(buildState, Parameters.Message);
            }

            if (!string.IsNullOrEmpty(Parameters.ButtonClick))
            {
                SetButtonClick(Parameters.ButtonClick);
            }

            if (Parameters.Off)
            {
                SwithAllOff();
            }

            if (Parameters.ReadFromJenkinsServer)
            {
                ReadFromJenkinsServer();
            }
        }

        private void ReadFromJenkinsServer()
        {
            var jenkensApi = new JenkensApi(AppSettings.Default.JenkenServer);
            var allProjects = jenkensApi.GetAllProjects();
            Log.Info("Downloading jenkins values");
            allProjects.Wait();
            var jenkensTextConverter = new JenkensTextConverter();
            var summaryList = jenkensTextConverter.ToSummaryList(allProjects.Result);
            var choreography = summaryList.Select(summary => new Choreography()
                {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesGpIo() {BeginTime = 0, Pin = AppSettings.Default.LsBluePin, IsOn = true},
                            new SequencesText2Speech() {BeginTime = 0, Text = summary},
                            new SequencesGpIo() {BeginTime = 1000, Pin = AppSettings.Default.LsBluePin, IsOn = false},
                        }
                }).ToArray();
            var result = CurrenRobotAmi.SetButtonChoreography(choreography);
            result.Wait();
        }

        private void SwithAllOff()
        {
            var result = CurrenRobotAmi.Enqueue(new Choreography()
                {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesGpIo() {Pin = AppSettings.Default.LsBluePin, IsOn = false},
                            new SequencesGpIo() {Pin = AppSettings.Default.LsGreenPin, IsOn = false},
                            new SequencesGpIo() {Pin = AppSettings.Default.LsRedPin, IsOn = false},
                            new SequencesGpIo() {Pin = AppSettings.Default.FeetGreenPin, IsOn = false},
                            new SequencesGpIo() {Pin = AppSettings.Default.FeetRedPin, IsOn = false}
                        }
                });
            result.Wait();
        }

        private void SetButtonClick(string message)
        {
            var choreography = new Choreography()
                {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesGpIo() { BeginTime = 0 ,Pin = AppSettings.Default.LsBluePin , IsOn =  true},
                            new SequencesText2Speech() {BeginTime = 0, Text = message},
                            new SequencesGpIo() { BeginTime = 1000,Pin = AppSettings.Default.LsBluePin , IsOn =  false},
                        }
                };
            var result = CurrenRobotAmi.SetButtonChoreography(choreography);
            result.Wait();
        }

        private States GetState()
        {
            var exception = new Exception(string.Format("Could not parse {0} into  [Success, Fail, InProgress]", Parameters.State));
            States state;
            try
            {
                state = (States) Enum.Parse(typeof (States), Parameters.State, true);
                if (state == States.Unknown)
                {
                
                    throw exception;
                }
            }
            catch (Exception)
            {
                throw exception;
            }
            return state;
            
        }

        private void SetGlow(States buildState, string message)
        {
            Choreography choreography = null;
            switch (buildState)
            {
                case States.Success:
                    choreography = new Choreography()
                    {
                        Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetGreenPin,
                                            IsOn = true
                                        },
                                        new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetRedPin,
                                            IsOn = false
                                        },
                                    
                                }
                    };
                    break;
                case States.Fail:
                    choreography = new Choreography()
                    {
                        Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetRedPin,
                                            IsOn = true
                                        },
                                        new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetGreenPin,
                                            IsOn = false
                                        },
                                    
                                }
                    };
                    break;
                case States.InProgress:
                    choreography = new Choreography()
                    {
                        Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetGreenPin,
                                            IsOn = true
                                        },
                                        new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetRedPin,
                                            IsOn = true
                                        },
                                }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("setBuildState");
            }
            AddMessage(choreography, Parameters.Message);
            var result = CurrenRobotAmi.Enqueue(choreography);
            result.Wait();
        }
        private void SetLightSaber(States setBuildState, string message)
        {
            Choreography choreography = null;
            switch (setBuildState)
            {
                case States.Success:
                    choreography = new Choreography()
                        {
                            Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsGreenPin,
                                            IsOn = true
                                        },
                                        new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = false
                                        },
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsRedPin,
                                            IsOn = false
                                        },
                                    new SequencesPlaySound() {BeginTime = 2000, File = "Success"},
                                }
                        };
                    break;
                case States.Fail:
                    choreography = new Choreography()
                    {
                        Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsRedPin,
                                            IsOn = true
                                        },
                                        new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = false
                                        },
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsGreenPin,
                                            IsOn = false
                                        },
                                    new SequencesPlaySound() {BeginTime = 2000, File = "Fail"},
                                }
                    };
                    break;
                case States.InProgress:
                    choreography = new Choreography()
                    {
                        Sequences = new List<Sequences>()
                                {
                                    new SequencesGpIo()
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = true
                                        },
                                    new SequencesPlaySound() {BeginTime = 1, File = "Wtf/r2d2_01.wav"},
                                }
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("setBuildState");
            }
            AddMessage(choreography, Parameters.Message);
            var result = CurrenRobotAmi.Enqueue(choreography);
            result.Wait();
        }

        private void AddMessage(Choreography choreography, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var max = choreography.Sequences.Select(x => x.BeginTime).Max();
                choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = max + 1000, Text = message});
            }
        }

        private enum States
        {
            Unknown,
            Success,
            Fail,
            InProgress
        }

        private void RunPassive()
        {
            var result = CurrenRobotAmi.PassiveProcess(new Passive()
                {
                    Interval = AppSettings.Default.PassiveInterval,
                    StartTime = AppSettings.Default.PassiveStartHour,
                    SleepTime = AppSettings.Default.PassiveStopHour,
                    Compositions = new List<Choreography>()
                        {
                            new Choreography()
                                {
                                    Sequences = new List<Sequences>()
                                        {
                                            new SequencesPlaySound() {File = "Funny"},
                                        }
                                },
                            new Choreography()
                                {
                                    Sequences = new List<Sequences>()
                                        {
                                            new SequencesPlaySound() {File = "Wtf"},
                                        }
                                }
                        }
                });
            result.Wait();

        }

        protected IRobotApi CurrenRobotAmi
        {
            get
            {
                if (_robotAmi == null)
                {
                    
                    _robotAmi = new RobotApi(HostApi);
                }
                return _robotAmi;
            }
        }

        protected string HostApi
        {
            get { return Parameters.Host ?? AppSettings.Default.Host; }
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