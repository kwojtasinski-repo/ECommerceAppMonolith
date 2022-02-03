using ECommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlerMiddleware>();
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
