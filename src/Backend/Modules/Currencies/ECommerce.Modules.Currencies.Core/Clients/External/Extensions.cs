using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared.Infrastructure;
using ECommerce.Modules.Currencies.Core.Exceptions;

namespace ECommerce.Modules.Currencies.Core.Clients.External
{
    internal static class Extensions
    {
        internal static IServiceCollection AddNbpClient(this IServiceCollection services)
        {
            var options = services.GetOptions<NbpClientOptions>("nbpClient");

            Validate(options);

            if (options.Timeout <= 0)
            {
                options.Timeout = 10;
            }

            services.Configure<NbpClientOptions>(config =>
            {
                config.BaseUrl = options.BaseUrl;
                config.Timeout = options.Timeout;
            });

            var timeout = TimeSpan.FromSeconds(options.Timeout);

            // http client
            services.AddHttpClient("NBPClient", options =>
            {
                options.Timeout = timeout;
                options.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler());

            services.AddScoped<INbpClient, NbpClient>();

            return services;
        }

        private static void Validate(NbpClientOptions clientOptions)
        {
            if (clientOptions is null)
            {
                throw new ClientOptionsCannotBeNullException();
            }

            if (string.IsNullOrWhiteSpace(clientOptions.BaseUrl))
            {
                throw new ClientOptionsBaseUrlCannotBeEmpty();
            }
        }
    }
}
