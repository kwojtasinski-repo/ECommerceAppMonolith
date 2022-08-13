using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Abstractions.Time;
using ECommerce.Shared.Infrastructure.Api;
using ECommerce.Shared.Infrastructure.Auth;
using ECommerce.Shared.Infrastructure.Commands;
using ECommerce.Shared.Infrastructure.Contexts;
using ECommerce.Shared.Infrastructure.Controllers;
using ECommerce.Shared.Infrastructure.Documentations;
using ECommerce.Shared.Infrastructure.Events;
using ECommerce.Shared.Infrastructure.Exceptions;
using ECommerce.Shared.Infrastructure.Kernel;
using ECommerce.Shared.Infrastructure.Messaging;
using ECommerce.Shared.Infrastructure.Modules;
using ECommerce.Shared.Infrastructure.Postgres;
using ECommerce.Shared.Infrastructure.Queries;
using ECommerce.Shared.Infrastructure.Services;
using ECommerce.Shared.Infrastructure.Time;
using ECommerce.Shared.Infrastructure.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("ECommerce.Bootstrapper")]
[assembly: InternalsVisibleTo("ECommerce.Shared.Tests")]
namespace ECommerce.Shared.Infrastructure
{
    internal static class Extensions
    {
        private const string modulePart = "ECommerce.Modules.";
        private const string CorsPolicy = "cors";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IList<Assembly> assemblies, IList<IModule> modules)
        {
            services.AddControllersFromModules();
            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins("*")
                          .WithMethods("POST", "PUT", "PATCH", "DELETE")
                          .WithHeaders("Content-Type", "Authorization")
                          .WithExposedHeaders("Location");
                });
            });

            services.AddSwagger();
            services.AddSingleton<IContextFactory, ContextFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => sp.GetRequiredService<IContextFactory>().Create());
            services.AddModuleInfo(modules);
            services.AddModuleRequests(assemblies);
            services.AddAuth(modules);
            services.AddErrorHandling();
            services.AddQueries(assemblies);
            services.AddCommands(assemblies);
            services.AddDomainEvents(assemblies);
            services.AddEvents(assemblies);
            services.AddMessaging();
            services.AddValidators(assemblies);
            services.AddPostgres();
            services.AddTransactionalDecorators();
            services.AddSingleton<IClock, UtcClock>();
            services.AddHostedService<AppInitializer>();

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseCors(CorsPolicy);
            app.UseErrorHandling();
            app.UseSwagger();
            app.UseSwaggerUserInterface();
            app.UseReDocDocumentation();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.MapGet("/", context => context.Response.WriteAsync("E-Commerce API!"));
            app.MapModuleInfo();
            return app;
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

        private static void SetDefaultJsonSerializerOptions()
        {
            var globalOptions = (JsonSerializerOptions?)typeof(JsonSerializerOptions)
                .GetField(
                    "s_defaultOptions",
                    BindingFlags.Static | BindingFlags.NonPublic
                )?.GetValue(null);

            if (globalOptions == null)
                throw new InvalidOperationException("Could not find property for global JsonSerializerOptions");

            globalOptions.Converters.Add(new DateOnlyJsonConverter());
            globalOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }
    }
}