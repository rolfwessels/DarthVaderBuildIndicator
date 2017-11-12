using System;
using System.Net;

namespace MainSolutionTemplate.Api.WebApi.Filters
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
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