using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class VoiceEnhancer : IVoiceEnhancer
	{
		
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private string _player;
		private string _backgroundFile;
		private string _convert;

		public VoiceEnhancer(string backgroundFile, string speedEcho)
		{
			_backgroundFile = Path.GetFullPath(backgroundFile).AsPath();
			_player = PlatformHelper.IsLinux
						  ? @"play|-q -m ""{1}"" -v 5 ""{0}"" " + speedEcho
						  : @"C:\Program Files\VideoLAN\VLC\vlc.exe| ""{1}"" ""{0}"" ";
			_convert = PlatformHelper.IsLinux
						  ? @"sox|-v 0.4 ""{0}"" -r 16000 ""{1}"""
						  : @"C:\Program Files\VideoLAN\VLC\vlc.exe| ""{0}"" ";
		
		}
		
		#region Implementation of IMp3Player

	    public async Task PlayFile(string fileName)
	    {
	        var fullPath = Path.GetFullPath(fileName).AsPath();
	        var backgroundFile = await CreateBackgroundFile(_backgroundFile);
	        var replace = string.Format(_player, fullPath, backgroundFile);
	        _log.Info("Player: " + replace.Replace("|", " "));
	        ProcessHelper.Run(replace);
	    }

	    #endregion

		private Task<string> CreateBackgroundFile(string backgroundFile)
		{
		    return Task.Run(() =>
		    {
		        var newFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(backgroundFile),"background.mp3")).AsPath();
		        if (!File.Exists(newFile))
		        {
		            var replace = string.Format(_convert, backgroundFile, newFile);
		            _log.Info("Convert: " + replace.Replace("|", " "));
		            ProcessHelper.Run(replace);
		        }
		        return newFile;
		    });

		}
	}
}