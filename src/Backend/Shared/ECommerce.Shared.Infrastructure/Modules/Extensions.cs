using ECommerce.Shared.Abstractions.Events;
using ECommerce.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ECommerce.Shared.Infrastructure.Modules
{
    internal static class Extensions
    {
        internal static IServiceCollection AddModuleInfo(this IServiceCollection services, IList<IModule> modules)
        {
            var moduleInfoProvider = new ModuleInfoProvider();
            var moduleInfo = modules?.Select(m => new ModuleInfo(m.Name, m.Path, m.Policies ?? Enumerable.Empty<string>())) ?? Enumerable.Empty<ModuleInfo>();
            moduleInfoProvider.Modules.AddRange(moduleInfo);
            services.AddSingleton(moduleInfoProvider);
            return services;
        }

        internal static void MapModuleInfo(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapGet("modules", context =>
            {
                var moduleInfoProvider = context.RequestServices.GetRequiredService<ModuleInfoProvider>();
                return context.Response.WriteAsJsonAsync(moduleInfoProvider.Modules);
            });
        }

        internal static IHostBuilder ConfigureModules(this IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                foreach (var settings in GetSettings("*"))
                {
                    cfg.AddJsonFile(settings);
                }

                foreach (var settings in GetSettings($"*{ctx.HostingEnvironment.EnvironmentName}"))
                {
                    cfg.AddJsonFile(settings);
                }

                IEnumerable<string> GetSettings(string pattern)
                {
                    var files = Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath,
                        $"module.{pattern}.json", SearchOption.AllDirectories);
                    return files;
                }
            });

            return builder;
        }

        internal static IServiceCollection AddModuleRequests(this IServiceCollection services, IList<Assembly> assemblies)
        {
            services.AddModuleRegistry(assemblies);
            services.AddSingleton<IModuleClient, ModuleClient>();
            services.AddSingleton<IModuleSerializer, JsonModuleSerializer>();
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            return services;
        }

        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
        {
            var moduleSubscriber = app.ApplicationServices.GetRequiredService<IModuleSubscriber>();
            return moduleSubscriber;
        }

        private static void AddModuleRegistry(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var registry = new ModuleRegistry();

            var types = assemblies.SelectMany(t => t.GetTypes()).ToArray();
            var eventTypes = types.Where(et => et.IsClass && typeof(IEvent).IsAssignableFrom(et))
                .ToArray();

            services.AddSingleton<IModuleRegistry>(sp =>
            {
                var eventDispatcher = sp.GetRequiredService<IEventDispatcher>();
                var eventDispatcherType = eventDispatcher.GetType();

                foreach (var type in eventTypes)
                {
                    registry.AddBroadcastAction(type, @event =>
                        (Task)eventDispatcherType.GetMethod(nameof(eventDispatcher.PublishAsync))
                            ?.MakeGenericMethod(type)
                            .Invoke(eventDispatcher, new[] { @event }));
                }

                return registry;
            });
        }

        public static IEnumerable<string> GetAllDisabledModules(this IServiceCollection services)
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

            return disabledModules;
        }
    }
}
