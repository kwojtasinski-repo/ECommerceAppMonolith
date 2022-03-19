public class Program
{
    public static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("reverseProxy"));

        var app = builder.Build();
        app.MapGet("/", context => context.Response.WriteAsync("ECommerce Gateway!"));
        app.MapReverseProxy();

        var task = app.RunAsync();
        return task;
    }
}