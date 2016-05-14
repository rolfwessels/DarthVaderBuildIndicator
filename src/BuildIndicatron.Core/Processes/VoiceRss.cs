using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class VoiceRss : ITextToSpeech
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string UriToDownload = "http://api.voicerss.org/?key={0}&src={1}&hl=en-gb&f=16khz_16bit_mono";
		private const int MaxLength = 65;
		private readonly IDownloadToFile _downloader;
		private readonly IMp3Player _mp3Player;
	    private string _key;

	    public VoiceRss(IDownloadToFile downloader , IMp3Player mp3Player)
		{
			_downloader = downloader;
			_mp3Player = mp3Player;
		    _key = "1fd3734b81574c7d961c5e69f613cdda";
		}

		#region Implementation of ITextToSpeech

		public void Play(string text)
		{
			Play(text, _mp3Player);
		}

		public void Play(string text, IMp3Player voiceEnhancer)
		{

            var uri = new Uri(string.Format(UriToDownload, _key, Uri.EscapeUriString(text)));
            _log.Debug(string.Format("VoiceRss:Play Download [{0}]", uri));
		    var downloadToTempFile = _downloader.DownloadToTempFile(uri, text);
            voiceEnhancer.PlayFile(downloadToTempFile);
		
		}

		#endregion

	}
}