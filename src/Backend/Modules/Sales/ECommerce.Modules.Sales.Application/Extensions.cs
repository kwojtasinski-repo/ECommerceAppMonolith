﻿using ECommerce.Modules.Sales.Application.Orders.Policies;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Sales.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyAssemblyGen2")] // added to allow generate mocks of internals
namespace ECommerce.Modules.Sales.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IOrderDeletionPolicy, OrderDeletionPolicy>();
            services.AddTransient<IOrderPositionModificationPolicy, OrderPositionModificationPolicy>();
            return services;
        }
    }
}