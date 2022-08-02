using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.BusinessLogicLayer;
using Store.Sharing.Constants;

namespace Store.PresentationLayer.Middlewares//TODO spelling----
{
    public class ErrorHandingMiddleware
    {
        private RequestDelegate _next;

        private ILoggerFactory _loggerFactory;//TODO code style, modifier+++

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
            catch (CustomException customException)//TODO spelling+++
            {
                string jsonString = JsonSerializer.Serialize(customException.ErrorList);
                context.Response.StatusCode = (int)customException.StatusCode;
                await context.Response.WriteAsync(jsonString);
            }

            catch (Exception exception)//TODO spelling+++
            {
                _loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), Constants.Loger.FILE_AND_CATEGORY_NAME));//TODO wrong constant
                var logger = _loggerFactory.CreateLogger(Constants.Loger.FILE_AND_CATEGORY_NAME);//TODO wrong constant
                string log = $"{DateTime.Now}\n{exception.Message}\n{exception.StackTrace}\n{new string (Constants.Loger.LOG_LAYOUT_DELIMITER, Constants.Loger.DELIMITER_COUNT)}";
                logger.LogError(log);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(Constants.Error.SERVER_ERROR);
            }
        }
    }
}
