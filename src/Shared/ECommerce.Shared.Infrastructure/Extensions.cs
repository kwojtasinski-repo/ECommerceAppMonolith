using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Bootstrapper")]
namespace ECommerce.Shared.Infrastructure
{
    internal static class Extensions
    {
        private const string modulePart = "ECommerce.Modules.";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
            IList<Assembly> assemblies, IList<IModule> modules)
        {
            var dissabledModules = new List<string>();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var configurations = configuration.AsEnumerable();

                foreach(var (key, value) in configurations)
                {
                    if (!key.Contains("modules:enabled"))
                    {
                        continue;
                    }

                    if (!bool.Parse(value))
                    {
                        dissabledModules.Add(key.Split(":")[0]);
                    }   
                }
            }

            services.AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    var removedParts = new List<ApplicationPart>();

                    foreach (var disabeldModule in dissabledModules)
                    {
                        var parts = manager.ApplicationParts.Where(app => app.Name.Contains(disabeldModule, StringComparison.InvariantCultureIgnoreCase));
                        removedParts.AddRange(parts);
                    }

                    foreach (var removePart in removedParts)
                    {
                        manager.ApplicationParts.Remove(removePart);
                    }

                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider()); // zmiana definicji wykrywania controller
                });

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.MapGet("/", context => context.Response.WriteAsync("E-Commerce API!"));
            return app;
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

        public static string GetModuleName(this string value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            if (!value.Contains(".dll"))
            {
                return string.Empty;
            }

            if (!value.Contains(modulePart))
            {
                return string.Empty;
            }

            var modulePartSplitted = value.Split(modulePart)[1];
            var name = value.Contains('.') ? modulePartSplitted.Split(".")[0].ToLowerInvariant() : string.Empty;

            return name;
        }
        
        public static string GetModuleName(this object value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var name = value.GetType().GetModuleName();
            return name;
        }

        public static string GetModuleName(this Type type)
        {
            if(type?.Namespace is null)
            {
                return string.Empty;
            }

            return type.Namespace.StartsWith("ECommerce.Modules") ? type.Namespace.Split(".")[2].ToLowerInvariant() : string.Empty;
        }
    }
}