﻿using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPurchaseDataRepository, PurchaseDataRepository>();
            services.AddScoped<IUserModelRepository, UserModelRepository>();
            services.AddScoped<IUserCustomerMapRepository, UserCustomerMapRepository>();
            services.AddScoped<IWeekPredictionRepository, WeekPredictionRepository>();
            return services;
        }
    }
}
