using BuildIndicatron.Server.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BuildIndicatron.Server.Api
{
    public class WebApiSetup
    {
        public static void Setup(MvcOptions config)
        {
            config.Filters.Add(new CaptureExceptionFilter());
        }
    }
}