using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.BusinessLogicLayer;

namespace Store.PresentationLayer.Middlewares
{
    public class ErrorHandingMiddleware
    {
        private RequestDelegate _next;

        public ILoggerFactory _loggerFactory;

        public ErrorHandingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (CustomExeption customExeption)
            {
                string jsonString = JsonSerializer.Serialize(customExeption.ErrorList);
                context.Response.StatusCode = customExeption.StatusCode;
                await context.Response.WriteAsync(jsonString);

            }

            catch (Exception exeption)
            {
                _loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
                var logger = _loggerFactory.CreateLogger("FileLogger");
                string log = $"{DateTime.Now}\n{exeption.StackTrace}\n{new string ('*',100)}";
                logger.LogError(log);
            }
        }
    }
}
