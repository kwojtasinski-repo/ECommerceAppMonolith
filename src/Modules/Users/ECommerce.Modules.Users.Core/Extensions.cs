using ECommerce.Modules.Users.Core.DAL;
using ECommerce.Modules.Users.Core.DAL.Repositories;
using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Repositories;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Infrastructure.Postgres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Users.Api")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Users.Tests.Unit")]
[assembly: InternalsVisibleTo("ECommerce.Modules.Users.Tests.Integration")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  //dodane dla generowania mockow do internali
namespace ECommerce.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddPostgres<UsersDbContext>();
            return services;
        }
    }
}