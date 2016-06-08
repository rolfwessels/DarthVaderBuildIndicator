using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{
    

    public class SetVolumeContext : ReposonseFlowBase, IReposonseFlow
    {
        private readonly ITextToSpeech _textToSpeech;

        public SetVolumeContext(ITextToSpeech textToSpeech)
        {
            _textToSpeech = textToSpeech;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "set volume"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var extractStartsWith = ValueConverterHelper.ToInt(ExtractStartsWith(context, "set volume"),10);
            var result = string.Format("volume set to {0}", extractStartsWith);
            _textToSpeech.Play(result);
            return context.Respond(result);
        }

        public class ValueConverterHelper
        {
            public static int ToInt(string stringValue, int max = 100, int min = 0)
            {
                int val;
                int.TryParse(stringValue, out val);
                return Math.Max(min, Math.Min(max, val));
            }
        }

        #endregion
    }
}