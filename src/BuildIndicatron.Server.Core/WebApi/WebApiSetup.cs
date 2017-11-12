using BuildIndicatron.Server.Core.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BuildIndicatron.Server.Core.WebApi
{
  public class WebApiSetup
  {
    public static void Setup(MvcOptions config)
    {
      config.Filters.Add(new CaptureExceptionFilter());
    }
  }
}