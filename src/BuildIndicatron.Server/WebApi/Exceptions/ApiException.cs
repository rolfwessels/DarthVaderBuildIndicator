using System;
using System.Net;

namespace CoreDocker.Api.WebApi.Filters
{
  public class ApiException : Exception
  {
    public ApiException(string message)
      : this(message, null)
    {
    }

    public ApiException(string message, Exception innerException)
      : this(HttpStatusCode.InternalServerError, message, innerException)
    {
    }

    public ApiException(HttpStatusCode statuscode, string message, Exception innerException) : base(message, innerException)
    {
      HttpStatusCode = statuscode;
    }

    public HttpStatusCode HttpStatusCode { get; set; }
  }
}