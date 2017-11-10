using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class TellMeAQuotesContext : TextSplitterContextBase<TellMeAQuotesContext.Meta>, IWithHelpText
    {
        private readonly ITextToSpeech _textToSpeech;
        private readonly IVoiceEnhancer _enhancer;

        public TellMeAQuotesContext(ITextToSpeech textToSpeech, IVoiceEnhancer enhancer)
        {
            _textToSpeech = textToSpeech;
            _enhancer = enhancer;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter.Map("(tell)(ANYTHING)(quote)(ANYTHING)");
        }


        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context,
            Meta server)
        {
            var oneLiner = RandomTextHelper.Quotes;
            await context.Respond(oneLiner);
            await _textToSpeech.Play(oneLiner, _enhancer);
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage {Call = "tell me a quote", Description = "Tells a quote."};
        }

        #endregion

        #region Nested type: Meta

        public class Meta
        {
        }

        #endregion
    }
}