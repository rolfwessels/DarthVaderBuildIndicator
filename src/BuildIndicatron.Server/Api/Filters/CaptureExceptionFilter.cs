using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuildIndicatron.Server.Api.Filters
{
    public class CaptureExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Overrides of ExceptionFilterAttribute

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is MyApiException apiException)
                RespondWithTheExceptionMessage(context, apiException);
            else if (IsSomeSortOfValidationError(exception))
                RespondWithBadRequest(context, exception);
            else if (exception is ValidationException)
                RespondWithValidationRequest(context, exception as ValidationException);
            else
                RespondWithInternalServerException(context, exception);
            return base.OnExceptionAsync(context);
        }

        #endregion

        public bool IsSomeSortOfValidationError(Exception exception)
        {
            return exception is ValidationException ||
                   exception is ArgumentException;
        }

        #region Private Methods

        private void RespondWithTheExceptionMessage(ExceptionContext context, MyApiException exception)
        {
            var errorMessage = new ErrorMessage(exception.Message);
            context.Result = CreateResponse(exception.HttpStatusCode, errorMessage);
        }

        private void RespondWithBadRequest(ExceptionContext context, Exception exception)
        {
            var errorMessage = new ErrorMessage(exception.Message);
            context.Result = CreateResponse(HttpStatusCode.BadRequest, errorMessage);
        }

        private void RespondWithValidationRequest(ExceptionContext context,
            ValidationException validationException)
        {
            var errorMessage =
                new ErrorMessage(validationException.Message);
            context.Result = CreateResponse(HttpStatusCode.BadRequest, errorMessage);
        }

        private void RespondWithInternalServerException(ExceptionContext context, Exception exception)
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            var errorMessage =
                new ErrorMessage("An internal system error has occurred. The developers have been notified.");
            _log.Error(exception.Message, exception);
#if DEBUG
            errorMessage.AdditionalDetail = exception.Message;
#endif
            context.Result = CreateResponse(httpStatusCode, errorMessage);
        }

        private IActionResult CreateResponse(HttpStatusCode httpStatusCode, object errorMessage)
        {
            return new ObjectResult(errorMessage) {StatusCode = (int) httpStatusCode};
        }

        #endregion
    }

    public class ErrorMessage
    {
        public string Message { get; }
        public string AdditionalDetail { get; set; }

        public ErrorMessage(string message)
        {
            Message = message;
        }
    }

    public class MyApiException : Exception
    {
        public MyApiException(string message) : base(message)
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}