using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{
    

    public class SayContext : ReposonseFlowBase, IReposonseFlow
    {
        private readonly ITextToSpeech _textToSpeech;

        public SayContext(ITextToSpeech textToSpeech)
        {
            _textToSpeech = textToSpeech;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "say"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var extractStartsWith = ExtractStartsWith(context, "say");
            _textToSpeech.Play(extractStartsWith);
            return context.Respond(extractStartsWith);
        }

        #endregion
    }
}