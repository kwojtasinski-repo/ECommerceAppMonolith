
using ECommerce.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Users.Core.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserDataProvider, UserDataProvider>();
            return services;
        }
    }
}
