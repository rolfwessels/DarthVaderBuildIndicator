using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using NDesk.Options;
using log4net;

namespace BuildIndicatron.Console
{
    public class CommandSimple : CommandBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected bool Off { get; set; }

        protected bool JenkensRead { get; set; }

        protected string ButtonClick { get; set; }

        protected string Glow { get; set; }

        protected string LightSaber { get; set; }
        
        protected bool SetPassive { get; set; }

        protected string Sound { get; set; }

        public CommandSimple()
        {
            IsCommand("send", "Send simple commands to build indicator");
            HasAdditionalArguments(0);
            var optionSet = new OptionSet
                {
                    {"setpassive", "Set the passive", s => SetPassive = true},
                    {"ls=", "Set state for light saber [RGB]", s => LightSaber = s},
                    {"glow=", "Set state for lower glow [RG]", s => Glow = s},
                    {"sound=", "add sound to play", s => Sound = s},
                    {"button=", "Text to be read when the button gets clicked", s => ButtonClick = s},
                    {"j", "Reads the values from jenkins service", s => JenkensRead = true},
                    {"off", "Stop all lights", s => Off = true},
                };
            
            foreach (var option in optionSet)
            {
                Options.Add(option);
            }
        }

        
        #region Overrides of CommandBase

        protected override int RunCommand(string[] remainingArguments)
        {
            if (SetPassive)
            {
                RunPassive();
            }

            var message = Message;
            if (remainingArguments.Length > 0) message = remainingArguments[0];
            if (!string.IsNullOrEmpty(LightSaber))
            {
                SetLightSaber(LightSaber, message);
            }

            else if (!string.IsNullOrEmpty(Glow))
            {

                SetGlow(Glow, message);
            }
            else
            {
                SendText(message);
            }


            if (!string.IsNullOrEmpty(ButtonClick))
            {
                SetButtonClick(ButtonClick);
            }

            if (Off)
            {
                SwithAllOff();
            }

            if (JenkensRead)
            {
                AddJenkensStatsToButton();
            }
            return 0;
        }

        private void SendText(string message)
        {
            Choreography choreography = null;
            choreography = new Choreography
            {
                Sequences = new List<Sequences>
                                {
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = true
                                        }
                                }
            };
            AddMessage(choreography, message);
            choreography.Sequences.Add(new SequencesGpIo
                                        {
                                            BeginTime = 1000,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = false
                                        });
            BuildIndicationApi.Enqueue(choreography).Wait();
        }

        #endregion


        #region Private Methods

        private void SwithAllOff()
        {
            Task<EnqueueResponse> result = BuildIndicationApi.Enqueue(new Choreography
            {
                Sequences = new List<Sequences>
                        {
                            new SequencesGpIo {Pin = AppSettings.Default.LsBluePin, IsOn = false},
                            new SequencesGpIo {Pin = AppSettings.Default.LsGreenPin, IsOn = false},
                            new SequencesGpIo {Pin = AppSettings.Default.LsRedPin, IsOn = false},
                            new SequencesGpIo {Pin = AppSettings.Default.FeetGreenPin, IsOn = false},
                            new SequencesGpIo {Pin = AppSettings.Default.FeetRedPin, IsOn = false}
                        }
            });
            result.Wait();
        }

        private void SetButtonClick(string message)
        {
            var choreography = new Choreography
            {
                Sequences = new List<Sequences>
                        {
                            new SequencesGpIo {BeginTime = 0, Pin = AppSettings.Default.LsBluePin, IsOn = true},
                            new SequencesText2Speech {BeginTime = 0, Text = message},
                            new SequencesGpIo {BeginTime = 1000, Pin = AppSettings.Default.LsBluePin, IsOn = false},
                        }
            };
            Task<SetButtonChoreographyResponse> result = BuildIndicationApi.SetButtonChoreography(choreography);
            result.Wait();
        }

        private void SetGlow(string buildState, string message)
        {
            Choreography choreography = null;
            choreography = new Choreography
            {
                Sequences = new List<Sequences>
                                {
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetGreenPin,
                                            IsOn = buildState.ToUpper().Contains('G')
                                        },
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.FeetRedPin,
                                            IsOn = buildState.ToUpper().Contains('R')
                                        },
                                }
            };
            AddMessage(choreography, message);
            AddSound(choreography, Sound);
            Task<EnqueueResponse> result = BuildIndicationApi.Enqueue(choreography);
            result.Wait();
        }

        

        private void SetLightSaber(string setBuildState, string message)
        {
            Choreography choreography = null;
            choreography = new Choreography
            {
                Sequences = new List<Sequences>
                                {
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsGreenPin,
                                            IsOn = setBuildState.ToUpper().Contains('G')
                                        },
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = setBuildState.ToUpper().Contains('B')
                                        },
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsRedPin,
                                            IsOn = setBuildState.ToUpper().Contains('R')
                                        },
                                }
            };
            AddMessage(choreography, message);
            AddSound(choreography, Sound);
            Task<EnqueueResponse> result = BuildIndicationApi.Enqueue(choreography);
            result.Wait();
        }

        private void AddMessage(Choreography choreography, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                int max = choreography.Sequences.Select(x => x.BeginTime).Max();
                choreography.Sequences.Add(new SequencesText2Speech { BeginTime = max + 1000, Text = message });
            }
        }

        private void AddSound(Choreography choreography, string sound)
        {
            if (string.IsNullOrEmpty(sound)) return;
            int max = choreography.Sequences.Select(x => x.BeginTime).Max();
            choreography.Sequences.Add(new SequencesPlaySound() { BeginTime = max + 1000, File = sound });
        }

        private void RunPassive()
        {
            Task<PassiveProcessResponse> result = BuildIndicationApi.PassiveProcess(new Passive
            {
                Interval = AppSettings.Default.PassiveInterval,
                StartTime = AppSettings.Default.PassiveStartHour,
                SleepTime = AppSettings.Default.PassiveStopHour,
                Compositions = new List<Choreography>
                        {
                            new Choreography
                                {
                                    Sequences = new List<Sequences>
                                        {
                                            new SequencesPlaySound {File = "Funny"},
                                        }
                                },
                            new Choreography
                                {
                                    Sequences = new List<Sequences>
                                        {
                                            new SequencesPlaySound {File = "Wtf"},
                                        }
                                },
                            new Choreography
                                {
                                    Sequences = new List<Sequences>
                                        {
                                            new SequencesQuotes(),
                                        }
                                }
                            ,
                            new Choreography
                                {
                                    Sequences = new List<Sequences>
                                        {
                                            new SequencesOneLiner(),
                                            new SequencesPlaySound {File = "Stop/jabba_laugh.wav"},
                                        }
                                }

                        }
            });
            result.Wait();
            System.Console.Out.WriteLine(
                string.Format("Passive has been set with intercal {0} starting at {1} and ending at {2}",
                              AppSettings.Default.PassiveInterval, AppSettings.Default.PassiveStartHour,
                              AppSettings.Default.PassiveStopHour));
        }


        private enum States
        {
            Unknown,
            Success,
            Fail,
            InProgress
        }

        #endregion
    }
}