using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ECommerce.Modules.Contacts.Api")]
namespace ECommerce.Modules.Contacts.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            return services;
        }
    }
}