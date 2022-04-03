using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Modules.Items.Infrastructure.EF.DAL.Repositories;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Infrastructure.Files.Implementations;
using ECommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Items.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyAssemblyGen2")] // added to allow generate mocks
namespace ECommerce.Modules.Items.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddPostgres<ItemsDbContext>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemSaleRepository, ItemSaleRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddTransient<IFileStore, FileStore>();
            services.AddTransient<IFileWrapper, FileWrapper>();
            services.AddTransient<IDirectoryWrapper, DirectoryWrapper>();
            return services;
        }
    }
}