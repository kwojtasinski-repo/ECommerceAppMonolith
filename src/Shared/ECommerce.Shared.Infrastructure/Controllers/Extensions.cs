using ECommerce.Shared.Abstractions.Time;
using ECommerce.Shared.Infrastructure.Api;
using ECommerce.Shared.Infrastructure.Conventions;
using ECommerce.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.Controllers
{
    internal static class Extensions
    {
        public static IServiceCollection AddControllersFromModules(this IServiceCollection services)
        {
            var disabledModules = new List<string>();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var configurations = configuration.AsEnumerable();

                foreach (var (key, value) in configurations)
                {
                    if (!key.Contains("modules:enabled"))
                    {
                        continue;
                    }

                    if (!bool.Parse(value))
                    {
                        disabledModules.Add(key.Split(":")[0]);
                    }
                }
            }
            services.AddControllers(options =>
            {
                options.UseDateOnlyTimeOnlyStringConverters();
                options.Conventions.Add(new RouteTokenTransformerConvention(new DashedConvention()));
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    var removedParts = new List<ApplicationPart>();

                    foreach (var disabledModule in disabledModules)
                    {
                        var parts = manager.ApplicationParts.Where(app => app.Name.Contains(disabledModule, StringComparison.InvariantCultureIgnoreCase));
                        removedParts.AddRange(parts);
                    }

                    foreach (var removePart in removedParts)
                    {
                        manager.ApplicationParts.Remove(removePart);
                    }

                    // change detection of controllers
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                });

            return services;
        }
    }
}
