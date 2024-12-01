using ECommerce.Bootstrapper;
using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure;
using System.Reflection;

public class Program
{
    public static Task Main(string[] args)
    {
        var builder = CreateModulesBuilder(args);
        var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
        var modules = ModuleLoader.LoadModules(assemblies);
        // Add services to the container.
        RegisterServices(builder.Services, assemblies, modules);
        return RunApplication(builder, assemblies, modules);
    }

    protected static Task RunApplication(WebApplicationBuilder builder, IList<Assembly> assemblies, IList<IModule> modules)
    {
        var app = builder.Build();
        app.Logger.LogInformation($"Modules {string.Join(",", modules.Select(m => m.Name))}");
        app.UseInfrastructure();

        foreach (var module in modules)
        {
            module.Use(app);
        }

        assemblies.Clear();
        modules.Clear();
        return app.RunAsync();
    }

    protected static WebApplicationBuilder CreateModulesBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.ConfigureModules();
        return builder;
    }

    protected static void RegisterServices(IServiceCollection services, IList<Assembly> assemblies, IList<IModule> modules)
    {
        services.AddInfrastructure(assemblies, modules);
        foreach (var module in modules)
        {
            module.Register(services);
        }
        services.AddSwaggerGen();
    }
}