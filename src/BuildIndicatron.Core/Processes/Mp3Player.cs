using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
                : @"C:\Program Files\VideoLAN\VLC\vlc.exe|""{0}""  vlc:quit";
        }

        #region Implementation of IMp3Player

        public Task PlayFile(string fileName)
        {
            return Task.Run(() =>
            {
                var fullPath = Path.GetFullPath(fileName).AsPath();
                _log.Info("Player: " + string.Format(_player, fullPath).Replace("|", " "));
                var playerCommand = _player.Split('|');
                ProcessHelper.Run(playerCommand[0], string.Format(playerCommand[1], fullPath));
            });
        }

        #endregion
    }
}