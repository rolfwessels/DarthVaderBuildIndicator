using CoreDocker.Api.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CoreDocker.Api.WebApi
{
  public class WebApiSetup
  {
    public static void Setup(MvcOptions config)
    {
      config.Filters.Add(new CaptureExceptionFilter());
    }
  }
}