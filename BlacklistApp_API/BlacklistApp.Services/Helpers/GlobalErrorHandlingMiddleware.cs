using BlacklistApp.Services.Models;
using Microsoft.AspNetCore.Http;
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
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
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
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
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
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
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

            Result(false, $"Error: {message}; StackTrace: {stackTrace}", (int)status));
            //{
            //    error = message,
            //    stackTrace
            //});
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
