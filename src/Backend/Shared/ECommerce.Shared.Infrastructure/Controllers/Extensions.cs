using ECommerce.Shared.Abstractions.Time;
using ECommerce.Shared.Infrastructure.Api;
using ECommerce.Shared.Infrastructure.Conventions;
using ECommerce.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.Controllers
{
    internal static class Extensions
    {
        public static ApplicationPartManager UseInternalControllers(this ApplicationPartManager applicationPartManager)
        {
            // change detection of controllers
            applicationPartManager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            return applicationPartManager;
        }

        public static MvcOptions UseDashedConventionInRouting(this MvcOptions options)
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new DashedConvention()));
            return options;
        }

        public static IMvcBuilder AddAllControllers(this IServiceCollection services, IEnumerable<string> disabledModules)
        {
            var mvcBuilder = services.AddControllers(options =>
            {
                options.UseDateOnlyTimeOnlyStringConverters();
                options.UseDashedConventionInRouting();
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
                    manager.UseInternalControllers();
                });

            return mvcBuilder;
        }
    }
}
