using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Abstractions.Time;
using ECommerce.Shared.Infrastructure.Api;
using ECommerce.Shared.Infrastructure.Auth;
using ECommerce.Shared.Infrastructure.Commands;
using ECommerce.Shared.Infrastructure.Contexts;
using ECommerce.Shared.Infrastructure.Conventions;
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
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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

            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, policy =>
                {
                    policy.WithOrigins("*")
                          .WithMethods("POST", "PUT", "PATCH", "DELETE")
                          .WithHeaders("Content-Type", "Authorization");
                });
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds(s => s.FullName);
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerce API",
                    Version = "v1"
                });
            });

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

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseCors(CorsPolicy);
            app.UseErrorHandling(); 
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUI =>
            {
                swaggerUI.RoutePrefix = "swagger";
                swaggerUI.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
                swaggerUI.DocumentTitle = "ECommerce API";
            });
            app.UseReDoc(reDoc =>
            {
                reDoc.RoutePrefix = "docs";
                reDoc.SpecUrl("/swagger/v1/swagger.json");
                reDoc.DocumentTitle = "ECommerce API";
            });
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