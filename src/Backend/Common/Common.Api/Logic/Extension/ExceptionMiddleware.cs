using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading;

namespace Common.Api.Logic.Extension
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(httpContext, ex);
            }
        }

        private async void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var responseError = new
            {
                success = false,
                errors = new List<string> { "Erro Interno.\n Inner Exception: " + exception.InnerException + "\n\nMessage: " + exception.Message }
            };

            if (exception.Message.Contains("contains authorization metadata"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(responseError);
            }
            else if (exception.Message.Contains("No connection could be made because the target machine actively refused it.") ||
                     exception.Message.Contains("The circuit is now open and is not allowing calls.") ||
                     exception.Message.Contains("Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente."))
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                await context.Response.WriteAsJsonAsync(responseError);
            }
            else
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync("InternalServerError: " + exception.Message + "\n\nStackTrace: " + exception.StackTrace +
                        "\n\nInnerException: " + exception.InnerException);

            }

        }
    }
}
