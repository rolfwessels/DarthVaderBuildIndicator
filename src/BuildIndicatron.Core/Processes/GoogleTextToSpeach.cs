using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class GoogleTextToSpeach : ITextToSpeech
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public const string UriToDownload = "http://translate.google.com/translate_tts?tl=en&q={0}";
		private const int MaxLength = 65;
		private readonly IDownloadToFile _downloader;
		private readonly IMp3Player _mp3Player;

		public GoogleTextToSpeach(IDownloadToFile downloader , IMp3Player mp3Player)
		{
			_downloader = downloader;
			_mp3Player = mp3Player;
		}

		#region Implementation of ITextToSpeech

		public Task Play(string text)
		{
			return Play(text, _mp3Player);
		}

		public async Task Play(string text, IMp3Player voiceEnhancer)
		{
			var stringToSends = Split(text).ToArray();
			var tasks = stringToSends.Select(stringToSend => Task.Run(() =>
				{
					var uri = new Uri(string.Format(UriToDownload, Uri.EscapeUriString(stringToSend)));
					_log.Debug(string.Format("GoogleTextToSpeach:Play Download [{0}]", uri));
					return _downloader.DownloadToTempFile(uri, stringToSend);
				})).ToList();
            await Task.WhenAny(tasks);
			foreach (var task in tasks)
			{
#pragma warning disable 4014
				voiceEnhancer.PlayFile(task.Result);
#pragma warning restore 4014
			}
		
		}

	    #endregion

		public IEnumerable<string> Split(string text)
		{	
			bool continueWhile = true;
			while (continueWhile)
			{
				if (!string.IsNullOrEmpty(text) && text.Length > MaxLength)
				{
					var nextSplit = text.LastCharInRange('.', MaxLength);
					if (nextSplit == -1) nextSplit = text.LastCharInRange(' ', MaxLength);
					if (nextSplit == -1) nextSplit = MaxLength;
					yield return text.Substring(0, nextSplit+1).Trim();
					text = text.Substring(nextSplit+1).Trim();
				}
				else
				{
					yield return text;
					continueWhile = false;
				}
			}
		}
	}
}