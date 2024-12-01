using ECommerce.Shared.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    public class TestCurrenciesAppFactory : TestApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHostedService<DataInitializer>();
            });
            base.ConfigureWebHost(builder);
        }
    }
}
