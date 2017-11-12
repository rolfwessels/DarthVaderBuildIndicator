using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Server.Properties;
using log4net;
using Microsoft.Extensions.DependencyInjection;

namespace BuildIndicatron.Server.Setup
{
    public static class BootStrap
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool _isInitialized;
        private static readonly object _locker = new object();
        private static SlackBotServer _slackBotServer;

        public static void Initialize(IServiceCollection app)
        {
            lock (_locker)
            {
                if (_isInitialized) return;
                _isInitialized = true;
                TaskHelper.LogExceptions(Task.Run(() => StartSlackBot()),"SlackBot starting");
                TaskHelper.LogExceptions(Task.Run(() => MonitorJenkins()), "MonitorJenkins starting");
                
            }
        }

        #region Private Methods

        private static void MonitorJenkins()
        {
            var monitorJenkins = IocContainer.Instance.Resolve<IMonitorJenkins>();
            monitorJenkins.StartMonitor(TimeSpan.FromSeconds(30));
        }

        private static void StartSlackBot()
        {
            
            var apiToken = Settings.Default.SlackKey;
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