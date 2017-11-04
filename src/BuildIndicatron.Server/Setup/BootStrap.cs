using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
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
				if (!_isInitialized)
				{
					_isInitialized = true;
				    var settings = IocContainer.Instance.Resolve<ISettingsManager>();
				    var apiToken = settings.Get("slack_token");
                    _log.Info(string.Format("Token:'{0}'", apiToken));
				    _slackBotServer = new SlackBotServer(apiToken);
                    _slackBotServer.ContinueslyTryToConnect().ContinueWith(task =>
                    {
                        var localIpAddress = IpAddressHelper.GetLocalIpAddresses().ToArray();
                        if (!localIpAddress.Any(x => x.Contains("192.168.1")))
                        _slackBotServer.SayTo("@rolf", "I'm on " + localIpAddress.StringJoin(" or "));
                        _log.Info("I'm on " + localIpAddress.StringJoin(" or "));
                    });
				    var monitorJenkins = IocContainer.Instance.Resolve<IMonitorJenkins>();
				    monitorJenkins.StartMonitor(TimeSpan.FromSeconds(30));
				}
			}
		}

	 
	}
}