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
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  //dodane dla generowania mockow do internali
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

            services.AddScoped<ISchedulerTask<CurrencyRateDownloader>, CurrencyRateDownloader>();
            services.AddCronJob<ISchedulerTask<CurrencyRateDownloader>, CurrencyRateDownloader >(options =>
            {
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronExpression = @"45 17 * * *";// https://crontab.guru/ info
            });

            return services;
        }
    }
}