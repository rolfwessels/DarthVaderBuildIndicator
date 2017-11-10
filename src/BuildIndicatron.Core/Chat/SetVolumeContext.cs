using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{
    public class SetVolumeContext : ReposonseFlowBase, IReposonseFlow
    {
        private readonly ITextToSpeech _textToSpeech;
        private readonly IVolumeSetter _volumeSetter;

        public SetVolumeContext(ITextToSpeech textToSpeech, IVolumeSetter volumeSetter)
        {
            _textToSpeech = textToSpeech;
            _volumeSetter = volumeSetter;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "set volume"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var volumeLevel = ValueConverterHelper.ToInt(ExtractStartsWith(context, "set volume"), 10);
            var result = string.Format("volume set to {0}", volumeLevel);
            _textToSpeech.Play(result);
            _volumeSetter.SetVolume(volumeLevel);
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