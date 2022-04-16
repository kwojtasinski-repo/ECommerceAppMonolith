using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.DAL.Repositories;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Modules.Currencies.Core.Scheduler;
using ECommerce.Modules.Currencies.Core.Services;
using ECommerce.Shared.Abstractions.SchedulerJobs;
using ECommerce.Shared.Infrastructure.Postgres;
using ECommerce.Shared.Infrastructure.SchedulerJobs;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Currencies.Api")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Currencies.Tests.Unit")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Currencies.Tests.Integration")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  // added to allow generate mocks of internal classes
namespace ECommerce.Modules.Currencies.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddNbpClient();
            //services.AddSingleton<ICurrencyRepository, InMemoryCurrencyRepository>();
            //services.AddSingleton<ICurrencyRateRepository, InMemoryCurrencyRateRepository>();
            services.AddPostgres<CurrenciesDbContext>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyRateService, CurrencyRateService>();

            services.AddCronJob<ISchedulerTask<CurrencyRateDownloader>, CurrencyRateDownloader>(options =>
            {
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronExpression = @"32 18 * * *";// https://crontab.guru/ info
            });

            return services;
        }
    }
}