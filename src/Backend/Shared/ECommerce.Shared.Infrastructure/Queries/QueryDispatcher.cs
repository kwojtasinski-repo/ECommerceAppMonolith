using ECommerce.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Queries
{
    internal class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            if (query is null)
            {
                throw new ArgumentException("Query cannot be null.");
            }

            using var scope = _serviceProvider.CreateScope();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var result = await (Task<TResult>)handlerType
                .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
                ?.Invoke(handler, new[] { query });
            
            return result;
        }
    }
}
