using System.Collections;
using System.Reflection;
using System.Web.Http;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using log4net;

namespace BuildIndicatron.Server.Controllers
{
	public class PingController : ApiController
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public PingResponse Get()
		{
			_log.Debug("PingController:Get Ping");
			return new PingResponse() { Version = typeof(PingController).Assembly.GetName().Version.ToString(), Platform = PlatformHelper.CurrentPlatform };
		}
	}
}