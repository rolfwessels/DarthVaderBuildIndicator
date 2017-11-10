using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{
    public class SayContext : ReposonseFlowBase, IReposonseFlow, IWithHelpText
    {
        private readonly ITextToSpeech _textToSpeech;
        private readonly IVoiceEnhancer _voiceEnhancer;

        public SayContext(ITextToSpeech textToSpeech, IVoiceEnhancer voiceEnhancer)
        {
            _textToSpeech = textToSpeech;
            _voiceEnhancer = voiceEnhancer;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "say"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var extractStartsWith = ExtractStartsWith(context, "say");
            _textToSpeech.Play(extractStartsWith, _voiceEnhancer);
            return context.Respond(extractStartsWith);
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "say _context_", Description = "Allow robot to say _context_."};
        }

        #endregion
    }
}