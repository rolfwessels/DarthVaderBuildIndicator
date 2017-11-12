using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Server.Core.Properties;
using log4net;

namespace BuildIndicatron.Server.Core.Setup
{
    public static class BootStrap
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static SlackBotServer _slackBotServer;

        public static void StartMonitoringJenking()
        {
            TaskHelper.LogExceptions(Task.Run(() => MonitorJenkins()), "MonitorJenkins starting");
        }

        public static void StartSlackBot()
        {
            TaskHelper.LogExceptions(Task.Run(() => RunSlackBot()), "SlackBot starting");
        }

        #region Private Methods

        private static void MonitorJenkins()
        {
            var monitorJenkins = IocContainer.Instance.Resolve<IMonitorJenkins>();
            monitorJenkins.StartMonitor(TimeSpan.FromSeconds(30));
        }

        private static void RunSlackBot()
        {
            var apiToken = ServerSettings.Default.SlackKey;
            _log.Info(string.Format("Token:'{0}'", apiToken.MaskInput()));
            if (!string.IsNullOrEmpty(apiToken))
            {
                _log.Debug("SlackBot: Starting server");
                _slackBotServer = new SlackBotServer(apiToken);
                _slackBotServer.ContinueslyTryToConnect().ContinueWith(task =>
                {
                    var localIpAddress = IpAddressHelper.GetLocalIpAddresses().ToArray();
                    if (!localIpAddress.Any(x => x.Contains("192.168.1")))
                        _slackBotServer.SayTo("@rolf", "I'm on " + localIpAddress.StringJoin(" or "));
                    _log.Info("I'm on " + localIpAddress.StringJoin(" or "));
                });
            }
            else
            {
                _log.Error("BootStrap:StartSlackBot apiToken is null or empty.");
            }
        }

        #endregion
    }
}