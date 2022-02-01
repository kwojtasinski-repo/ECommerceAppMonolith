using ECommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddScoped<ErrorEventHandler>();
            services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
            services.AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
            return services;
        }

        public static WebApplication UseErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }       
    }
}
