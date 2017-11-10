using System;
using System.Reflection;
using BuildIndicatron.Core.Chat;
using log4net;
using log4net.Core;

namespace BuildIndicatron.Core.Helpers
{
    public class VolumeSetter : IVolumeSetter
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void SetVolume(int volumeLevel)
        {
            var fileName = "amixer";
            var arguments = String.Format("cset numid=1 -- {0}%", volumeLevel.Map(0, 10, 10, 120));
            _log.Info(string.Format("{0} {1}", fileName, arguments));
            ProcessHelper.Run(fileName, arguments);
        }
    }
}