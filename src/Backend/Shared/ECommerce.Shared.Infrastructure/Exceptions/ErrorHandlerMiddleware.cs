using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Exceptions
{
    internal class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IExceptionCompositionRoot _exceptionCompositionRoot;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, IExceptionCompositionRoot exceptionCompositionRoot)
        {
            _logger = logger;
            _exceptionCompositionRoot = exceptionCompositionRoot;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message);
                await HandlerErrorAsync(context, exception);
            }
        }

        private async Task HandlerErrorAsync(HttpContext context, Exception exception)
        {
            var errorResponse = _exceptionCompositionRoot.Map(exception);
            context.Response.StatusCode = (int) (errorResponse?.HttpStatusCode ?? HttpStatusCode.InternalServerError);
            var response = errorResponse?.Response;

            if (response is null)
            {
                return;
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
