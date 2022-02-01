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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Logger.LogInformation($"Modules {string.Join(",", modules.Select(m => m.Name))}");

        app.UseInfrastructure();

        assemblies.Clear();
        modules.Clear();

        var task = app.RunAsync();
        return task;
    }
}