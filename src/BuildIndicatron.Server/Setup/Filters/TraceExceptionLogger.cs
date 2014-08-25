using System.Reflection;
using System.Web.Http.ExceptionHandling;
using log4net;

namespace BuildIndicatron.Server.Setup.Filters
{
	public class TraceExceptionLogger : ExceptionLogger
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public override void Log(ExceptionLoggerContext context)
		{
			_log.Error(context.ExceptionContext.Exception.Message, context.ExceptionContext.Exception);
		}
	}
}