using ECommerce.Modules.Contacts.Core.DAL;
using ECommerce.Modules.Contacts.Core.DAL.Repositories;
using ECommerce.Modules.Contacts.Core.Repositories;
using ECommerce.Modules.Contacts.Core.Services;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Contacts.Api")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Contacts.Tests.Unit")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Contacts.Tests.Integration")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  //dodane dla generowania mockow do internali
namespace ECommerce.Modules.Contacts.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            //services.AddSingleton<IAddressRepository, InMemoryAddressRepository>();
            //services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
            services.AddPostgres<ContactsDbContext>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICustomerService, CustomerService>();
            return services;
        }
    }
}