using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class FailedToRespondContext : TextSplitterContextBase<FailedToRespondContext.Meta>
    {
        private readonly ITextToSpeech _textToSpeech;
        private readonly IVoiceEnhancer _enhancer;

        public FailedToRespondContext(ITextToSpeech textToSpeech, IVoiceEnhancer enhancer)
        {
            _textToSpeech = textToSpeech;
            _enhancer = enhancer;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter.Map("(ANYTHING)");
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Meta server)
        {
            var insult = string.Format("{0}, {1}", context.FromUser, RandomTextHelper.Insult);
            var strings = new [] {RandomTextHelper.DontUnderstand,RandomTextHelper.DontUnderstand,insult};
            await Process(context, strings.Random());
        }

        private async Task Process(IMessageContext context, string insult)
        {
            await context.Respond(insult);
            await _textToSpeech.Play(insult, _enhancer);
        }

        #endregion

        

        #region Nested type: Meta

        public class Meta
        {
        }

        #endregion
    }
}