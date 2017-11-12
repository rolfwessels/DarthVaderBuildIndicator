using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Shared.Models.ApiResponses;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BuildIndicatron.Server.Core.WebApi.Controllers
{
    [Route(RouteHelper.PingController)]
	public class PingController : Controller
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    public static string Env { get; set; }

	    public PingController(IHostingEnvironment hosting)
	    {
	        Env = hosting.EnvironmentName;

	    }

	    [HttpGet]
	    public PingResponse Get()
		{
			_log.Debug("PingController:Get Ping");
			return new PingResponse() { Version = typeof(PingController).Assembly.GetName().Version.ToString() , Environment = Env, Platform = PlatformHelper.CurrentPlatform };
		}
	}
}