using Microsoft.Extensions.DependencyInjection;
using ECommerce.Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ECommerce.Modules.Currencies.Core.Exceptions;
using Flurl.Http;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal static class Extensions
    {
        internal static IServiceCollection AddExternalClient(this IServiceCollection services)
        {
            var options = services.GetOptions<ClientOptions>("externalClient");

            Validate(options);

            if (options.Timeout <= 0)
            {
                options.Timeout = 10;
            }

            services.Configure<ClientOptions>(config =>
            {
                config.BaseUrl = options.BaseUrl;
                config.Timeout = options.Timeout;
            });

            FlurlHttp.ConfigureClient("", config => config.Configure(settings => 
            {
                var timeout = TimeSpan.FromSeconds(options.Timeout);
                settings.Timeout = timeout;
                
            }).WithHeaders(new
            {
                Accept = "application/json"
            }));

            return services;
        }

        private static void Validate(ClientOptions clientOptions)
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

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }
}
