using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlacklistApp.Services.Helpers
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<GlobalErrorHandlingMiddleware> logger)
        //{
        //    app.UseExceptionHandler(appError =>
        //    {
        //        appError.Run(async context =>
        //        {
        //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //            context.Response.ContentType = "application/json";
        //            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //            if (contextFeature != null)
        //            {
        //                logger.LogError($"Something went wrong: {contextFeature.Error}");
        //                await context.Response.WriteAsync(new ErrorDetails()
        //                {
        //                    StatusCode = context.Response.StatusCode,
        //                    Message = "Internal Server Error."
        //                }.ToString());
        //            }
        //        });
        //    });
        //}
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = String.Empty;
            string message;
            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
                stackTrace = exception.StackTrace;
            }
            else if (exception is UnauthorizedAccessException)
            {
                message = exception.Message;
                status = HttpStatusCode.Unauthorized;
                stackTrace = exception.StackTrace;
            }
            else if (exception is SecurityTokenException) // Example, you may have different JWT-related exceptions
            {
                message = exception.Message;
                status = HttpStatusCode.Forbidden;
                stackTrace = exception.StackTrace;
            }
            else if (exception is SecurityTokenExpiredException) // Example, you may have different JWT-related exceptions
            {
                message = exception.Message;
                status = HttpStatusCode.Unauthorized;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(InvalidCastException))
            {
                message = exception.Message;
                status = HttpStatusCode.InternalServerError;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(ArgumentException))
            {
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.Unauthorized;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }

            var exceptionResult = JsonSerializer.Serialize(new

            Result<object>(false, $"An error has occured.", new { ErrorMessage = message, StackTrace = stackTrace  }, (int)status));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(exceptionResult);
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }
        public class NotImplementedException : Exception
        {
            public NotImplementedException(string message) : base(message) { }
        }
        public class UnauthorizedAccessException : Exception
        {
            public UnauthorizedAccessException(string message) : base(message) { }
        }
    }
}
