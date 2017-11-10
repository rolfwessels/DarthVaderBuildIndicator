using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using log4net;

namespace BuildIndicatron.Server.Fakes
{
    public class FakePlayer : IVoiceEnhancer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Implementation of IMp3Player

        public Task PlayFile(string fileName)
        {
            return Task.Run(() => { _log.Info(string.Format("PLAYING: {0}", fileName)); });
        }

        #endregion
    }


    public class FakeTextToSpeech : ITextToSpeech
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Implementation of ITextToSpeech

        public Task Play(string text)
        {
            return Task.Run(() => { _log.Info(string.Format("text: [{0}]", text)); });
        }

        public Task Play(string text, IMp3Player voiceEnhancer)
        {
            return Task.Run(() => { _log.Info(string.Format("Using voice: [{0}]", text)); });
        }

        #endregion
    }
}