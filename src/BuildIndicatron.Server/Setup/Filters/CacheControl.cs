using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace BuildIndicatron.Server.Setup.Filters
{
	public class CacheControl : ActionFilterAttribute
	{
		public int MaxAge { get; set; }

		public CacheControl()
		{
			MaxAge = 3600;
		}

		public override void OnActionExecuted(HttpActionExecutedContext context)
		{
			context.Response.Headers.CacheControl = new CacheControlHeaderValue()
				{
					Public = true,
					MaxAge = TimeSpan.FromMilliseconds(0)
				};

			base.OnActionExecuted(context);
		}
	}
}