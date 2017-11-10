using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class TellMeAInsultContext : TextSplitterContextBase<TellMeAInsultContext.Meta>, IWithHelpText
    {
        private readonly ITextToSpeech _textToSpeech;
        private readonly IVoiceEnhancer _enhancer;

        public TellMeAInsultContext(ITextToSpeech textToSpeech, IVoiceEnhancer enhancer)
        {
            _textToSpeech = textToSpeech;
            _enhancer = enhancer;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter.Map("(tell)(ANYTHING)(insult)(ANYTHING)");
        }


        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context,
            Meta server)
        {
            var insult = RandomTextHelper.Insult;
            await context.Respond(insult);
            await _textToSpeech.Play(insult, _enhancer);
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage {Call = "tell me a insult", Description = "Tells a insult."};
        }

        #endregion

        #region Nested type: Meta

        public class Meta
        {
        }

        #endregion
    }
}