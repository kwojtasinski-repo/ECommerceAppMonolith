using ECommerce.Modules.Users.Core;
using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Services;
using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Modules.Users.Api
{
    public class UsersModule : IModule
    {
        public const string BasePath = "users-module";
        public string Name => "Users";

        public string Path => BasePath;

        public IEnumerable<string> Policies { get; } = new[]
        {
            "users"
        };

        public void Register(IServiceCollection services)
        {
            services.AddCore();
        }

        public void Use(IApplicationBuilder app)
        {
            app.UseModuleRequests()
                .Subscribe<GetUserDataDto, UserDataDto>("/users/get", (command, sp) =>
                    sp.GetRequiredService<IUserDataProvider>().GetUserDataAsync(command)
                );
        }
    }
}