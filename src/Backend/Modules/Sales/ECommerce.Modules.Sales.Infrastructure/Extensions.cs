using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Payments.Repositories;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Modules.Sales.Infrastructure.EF.Repositories;
using ECommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Sales.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //services.AddSingleton<IItemRepository, InMemoryItemRepository>();
            //services.AddSingleton<IItemSaleRepository, InMemoryItemSaleRepository>();
            services.AddPostgres<SalesDbContext>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemSaleRepository, ItemSaleRepository>();
            services.AddScoped<IItemCartRepository, ItemCartRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            services.AddUnitOfWork<ISalesUnitOfWork, SalesUnitOfWork>();
            return services;
        }
    }
}