using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace RetailWebApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;
            string logText = $"[{DateTime.Now}] EXCEPTION INTERCEPTED: {exceptionMessage}{Environment.NewLine}{context.Exception.StackTrace}{Environment.NewLine}";

            // Write exception logs to a physical local file
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "error_logs.txt");
            File.AppendAllText(logFilePath, logText);

            // Structure a clean error response body back to the client
            var errorResponse = new { Message = "An unexpected error occurred.", ErrorDetails = exceptionMessage };
            
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };

            // Inform the pipeline that the exception has been safely caught and handled
            context.ExceptionHandled = true;
        }
    }
}