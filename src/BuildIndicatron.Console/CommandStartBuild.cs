using System.Collections.Generic;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Console
{
    public class CommandStartBuild : CommandBase
    {
        public CommandStartBuild()
        {
            IsCommand("start", "Mark the start of the project");
            HasAdditionalArguments(1, "<Name of project>");
        }

        #region Overrides of CommandBase

        protected override int RunCommand(string[] remainingArguments)
        {
            var choreography = new Choreography
            {
                Sequences = new List<Sequences>
                                {
                                    new SequencesGpIo
                                        {
                                            BeginTime = 0,
                                            Pin = AppSettings.Default.LsBluePin,
                                            IsOn = true
                                        },
                                    new SequencesPlaySound {BeginTime = 1, File = "Wtf/lightsaber.mp3"},
                                }
            };
            BuildIndicationApi.Enqueue(choreography);
            return 0;
        }

        #endregion
    }
}