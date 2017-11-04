using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace BuildIndicatron.Server.Api.Filters
{
	public class CacheFilter : ActionFilterAttribute
	{
		public int MaxAge { get; set; }

		public CacheFilter()
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