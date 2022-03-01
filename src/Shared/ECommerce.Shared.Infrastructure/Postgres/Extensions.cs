using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Infrastructure.Postgres.Decorators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Postgres
{
    public static class Extensions
    {
        internal static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var options = services.GetOptions<PostgresOptions>("postgres");

            services.Configure<PostgresOptions>(config =>
            {
                config.ConnectionString = options.ConnectionString;
            });

            return services;
        }

        public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
        {
            var options = services.GetOptions<PostgresOptions>("postgres");
            services.AddDbContext<T>(context => context.UseNpgsql(options.ConnectionString, sqlQptions =>
            {
                sqlQptions.EnableRetryOnFailure();
            }));
            return services;
        }

        public static IServiceCollection AddTransactionalDecorators(this IServiceCollection services)
        {
            services.TryDecorate(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
            services.TryDecorate(typeof(ICommandHandler<,>), typeof(TransactionalCommandHandlerDecorator<>));
            return services;
        }

        public static IServiceCollection AddUnitOfWork<TUnitOfWork, TImplementationUnitOfWork>(this IServiceCollection services)
            where TUnitOfWork : class, IUnitOfWork
            where TImplementationUnitOfWork : class, TUnitOfWork
        {
            services.AddScoped<TUnitOfWork, TImplementationUnitOfWork>();
            services.AddScoped<IUnitOfWork, TImplementationUnitOfWork>();

            using var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<UnitOfWorkTypeRegistry>().Register<TUnitOfWork>();

            return services;
        }
    }
}
