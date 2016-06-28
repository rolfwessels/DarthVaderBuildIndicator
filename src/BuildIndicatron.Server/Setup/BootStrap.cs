using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Server.Setup.Filters;
using log4net;
using Owin;

namespace BuildIndicatron.Server.Setup
{
	public static class BootStrap
	{
	    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static bool _isInitialized;
		private static readonly object _locker = new object();
	    private static SlackBotServer _slackBotServer;

	    public static void Initialize(IAppBuilder app)
		{
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_isInitialized = true;
					ConfigureWebApi(app);
					ConfigureIndexResponse(app);
				    var settings = IocContainer.Instance.Resolve<ISettingsManager>();
                    _slackBotServer = new SlackBotServer(settings.Get("SlackToken"));
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

	    #region Private Methods

		private static void ConfigureWebApi(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());
			config.Filters.Add(new CacheControl());

			ConfigureRoutes(config);
			ConfigureTheDependencyInjection(config);
			app.UseWebApi(config);
		}

		private static void ConfigureIndexResponse(IAppBuilder app)
		{
			app.Run(context =>
				{
					if (context.Request.Path.Value == "/")
					{
						context.Response.ContentType = "text/html";
						return context.Response.WriteAsync("<html><body>Hello World! <a href=\"api/echo\">echo</a></body></html>");
					}

					context.Response.StatusCode = 404;
					return Task.Delay(0);
				});
		}

		private static void ConfigureTheDependencyInjection(HttpConfiguration config)
		{
			var resolver = new AutofacWebApiDependencyResolver(IocContainer.Instance);
			config.DependencyResolver = resolver;
		}

		private static void ConfigureRoutes(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "Default api",
				routeTemplate: "api/{controller}",
				defaults: new {id = RouteParameter.Optional}
				);
			
			config.Routes.MapHttpRoute(
				name: "Default api call with id",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional, action = "get" }
				);
			
			config.Routes.MapHttpRoute(
				name: "Default api call with id and action",
				routeTemplate: "api/TextToSpeech/{id}/{action}",
				defaults: new { id = "test", action = "EnhanceSpeech", controller = "TextToSpeech" }
				);

			config.Routes.MapHttpRoute(
				name: "Playmp3file with file name",
				routeTemplate: "api/soundplayer/{folder}/{file}",
				defaults: new { id = RouteParameter.Optional, action = "Get", controller = "SoundPlayer" }
				);
			
//			config.Routes.MapHttpRoute(
//				name: "OutputGpio with pin and ison",
//				routeTemplate: "api/ouputgpio/{pin}/{ison}",
//				defaults: new { id = RouteParameter.Optional, action = "Get", controller = "OutputGpio" }
//				);
//			
//			
		}

		#endregion
	}
}