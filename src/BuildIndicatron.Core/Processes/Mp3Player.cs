using System.Diagnostics;
using System.IO;
using System.Media;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using log4net;

namespace BuildIndicatron.Core.Processes
{
	public class Mp3Player : IMp3Player
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private string _player;

		public Mp3Player()
		{
			_player = PlatformHelper.IsLinux
						  ? @"/usr/bin/mpg321|-q ""{0}"""
						  : @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe|""{0}""  vlc:quit";
			
		}

		#region Implementation of IMp3Player

		public void PlayFile(string fileName)
		{
			var fullPath = Path.GetFullPath(fileName);
			_log.Info("Player: "+string.Format(_player, fullPath).Replace("|"," "));
			var playerCommand = _player.Split('|');
			var startInfo = new ProcessStartInfo(playerCommand[0])
				{
					WindowStyle = ProcessWindowStyle.Minimized,
					Arguments = string.Format(playerCommand[1], fullPath)
				};
			var process = Process.Start(startInfo);
			process.WaitForExit(30000);
		}

		#endregion
	}
}