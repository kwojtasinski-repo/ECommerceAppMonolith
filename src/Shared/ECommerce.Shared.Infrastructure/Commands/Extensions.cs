using ECommerce.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Shared.Infrastructure.Commands
{
    internal static class Extensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s => s.FromAssemblies(assemblies)
                                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                                .AsImplementedInterfaces()
                                .WithScopedLifetime());
            return services;
        }
    }
}
