using ECommerce.Modules.PurchaseProfiler.Core.Clients;
using ECommerce.Modules.PurchaseProfiler.Core.Database;
using ECommerce.Modules.PurchaseProfiler.Core.External;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.PurchaseProfiler.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.PurchaseProfiler.Tests.Integration")]
namespace ECommerce.Modules.PurchaseProfiler.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddArrangoDb();
            services.AddRepositories();
            services.AddClients();
            services.AddServices();
            services.AddProfilerClient();
            return services;
        }
    }
}
