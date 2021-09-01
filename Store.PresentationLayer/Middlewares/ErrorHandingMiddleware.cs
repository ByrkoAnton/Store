using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.BusinessLogicLayer;
using Store.Sharing.Constants;

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
                context.Response.StatusCode = (int)customExeption.StatusCode;
                await context.Response.WriteAsync(jsonString);
            }

            catch (Exception exeption)
            {
                _loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), Constants.Loger.FILE_NAME));
                var logger = _loggerFactory.CreateLogger(Constants.Loger.CATEGORY_NAME);
                string log = $"{DateTime.Now}\n{exeption.Message}\n{exeption.StackTrace}\n{new string (Constants.Loger.LOG_LAYOUT_DELIMITER, Constants.Loger.DELIMITER_COUNT)}";
                logger.LogError(log);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(Constants.Error.SERVER_ERROR);
            }
        }
    }
}
