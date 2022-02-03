using ECommerce.Bootstrapper;
using ECommerce.Shared.Infrastructure;

public class Program
{
    public static Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.ConfigureModules();

        var assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
        var modules = ModuleLoader.LoadModules(assemblies);
        // Add services to the container.
        builder.Services.AddInfrastructure(assemblies, modules);
        foreach (var module in modules)
        {
            module.Register(builder.Services);
        }
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.Logger.LogInformation($"Modules {string.Join(",", modules.Select(m => m.Name))}");
        app.UseInfrastructure();

        foreach (var module in modules)
        {
            module.Use(app);
        }

        assemblies.Clear();
        modules.Clear();

        var task = app.RunAsync();
        return task;
    }
}