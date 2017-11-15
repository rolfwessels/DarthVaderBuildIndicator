using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Filters;
using BuildIndicatron.Server.Pi.WebApi.Exceptions;
using BuildIndicatron.Shared.Models;
using log4net;

namespace BuildIndicatron.Server.Pi.WebApi.Filters
{
    public class CaptureExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext context)
        {
            Exception exception = context.Exception;

            var apiException = exception as ApiException;
            if (apiException != null)
            {
                RespondWithTheExceptionMessage(context, apiException);
            }
            
            else
            {
                RespondWithInternalServerException(context, exception);
            }
        }

        #region Private Methods

        private static void RespondWithTheExceptionMessage(HttpActionExecutedContext context, ApiException exception)
        {
            var errorMessage = new ErrorMessage(exception.Message);
            context.Response = context.Request.CreateResponse(exception.HttpStatusCode, errorMessage);
        }

        private static void RespondWithBadRequest(HttpActionExecutedContext context, Exception exception)
        {
            var errorMessage = new ErrorMessage(exception.Message);
            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
        }

       

        private void RespondWithInternalServerException(HttpActionExecutedContext context, Exception exception)
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            var errorMessage =
                new ErrorMessage("An internal system error has occurred. The developers have been notified.");
            _log.Error(exception.Message, exception);
#if DEBUG
            errorMessage.AdditionalDetail = exception.Message;
#endif
            context.Response = context.Request.CreateResponse(httpStatusCode, errorMessage);
        }

       

        #endregion
    }
}