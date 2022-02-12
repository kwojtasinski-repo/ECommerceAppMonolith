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
            return services;
        }
    }
}