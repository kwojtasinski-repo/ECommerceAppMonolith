using ECommerce.Shared.Abstractions.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Validators
{
    internal static class Extensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.Scan(s => s.FromAssemblies(assemblies)
                                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                                .AsImplementedInterfaces()
                                .WithSingletonLifetime());
            return services;
        }

    }
}
