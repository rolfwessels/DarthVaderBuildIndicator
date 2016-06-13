using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class SaySomethingContext : TextSplitterContextBase<SaySomethingContext.Meta>, IWithHelpText
    {
        private readonly IMp3Player _mp3Player;
        private readonly ISoundFilePicker _soundFilePicker;

        public SaySomethingContext(IMp3Player mp3Player, ISoundFilePicker soundFilePicker)
        {
            _mp3Player = mp3Player;
            _soundFilePicker = soundFilePicker;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter.Map("(say|play) (something) (?<lookup>WORD)(ANYTHING)");
        }


        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Meta server)
        {
            await context.Respond(string.Format("Playing something {0}.", server.Lookup));
            await _mp3Player.PlayFile(_soundFilePicker.PickFile(server.Lookup));
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage {Call = "say something _funny,success,fail_", Description = "Plays a clip"};
        }

        #endregion

        #region Nested type: Meta

        public class Meta
        {
            public string Lookup { get; set; }
        }

        #endregion
    }
}