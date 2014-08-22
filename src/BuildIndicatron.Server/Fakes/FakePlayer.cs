using System.Reflection;
using BuildIndicatron.Core.Processes;
using log4net;

namespace BuildIndicatron.Server.Fakes
{
	public class FakePlayer : IMp3Player
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		#region Implementation of IMp3Player

		public void PlayFile(string fileName)
		{
			_log.Info(string.Format("PLAYING: {0}", fileName));
		}

		#endregion
	}


	internal class FakeTextToSpeech : ITextToSpeech
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		#region Implementation of ITextToSpeech

		public void Play(string text)
		{
			_log.Info(string.Format("text: [{0}]", text));
		}

		#endregion
	}
}