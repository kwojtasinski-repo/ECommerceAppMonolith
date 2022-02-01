using ECommerce.Shared.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Exceptions
{
    /// <summary>
    /// klasa stworzona na cele implementacji wlasnych mapowan wyjatkow na konkretny response
    /// </summary>
    internal class ExceptionCompositionRoot : IExceptionCompositionRoot
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionCompositionRoot(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ExceptionResponse Map(Exception exception)
        {
            using var scope = _serviceProvider.CreateScope();
            var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToList();
            var nonDefaultMappers = mappers.Where(m => m is not ExceptionToResponseMapper);
            var result = nonDefaultMappers.Select(nonDefaultMappers => nonDefaultMappers.Map(exception))
                .SingleOrDefault(e => e is not null);

            if (result is not null)
            {
                return result;
            }

            var defaultMapper = mappers.SingleOrDefault(m => m is ExceptionToResponseMapper);
            return defaultMapper?.Map(exception);
        }
    }
}
