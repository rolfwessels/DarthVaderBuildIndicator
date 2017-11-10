using System;
using System.Linq;
using System.Reflection;
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
			    if (_isInitialized) return;
			    _isInitialized = true;
			    StartSlackBot();
			    MonitorJenkins();
			}
		}

	    private static void MonitorJenkins()
	    {
	        var monitorJenkins = IocContainer.Instance.Resolve<IMonitorJenkins>();
	        monitorJenkins.StartMonitor(TimeSpan.FromSeconds(30));
	    }

	    private static void StartSlackBot()
	    {
	        var settings = IocContainer.Instance.Resolve<ISettingsManager>();
	        var apiToken = settings.Get("slack_token");
	        _log.Info(string.Format("Token:'{0}'", MaskInput(apiToken)));
	        if (string.IsNullOrEmpty(apiToken))
	        {
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

	    private static string MaskInput(string input, int charactersToShowAtEnd = 5)
	    {
	        if (input == null) return null;
	        if (input.Length < charactersToShowAtEnd)
	        {
	            charactersToShowAtEnd = input.Length;
	        }
	        var endCharacters = input.Substring(input.Length - charactersToShowAtEnd);
	        return string.Format("{0}{1}","".PadLeft(input.Length - charactersToShowAtEnd, '*')+endCharacters
	        );
	    }
    }
}