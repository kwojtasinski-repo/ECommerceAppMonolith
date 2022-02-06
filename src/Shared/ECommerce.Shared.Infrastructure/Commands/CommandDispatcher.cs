using ECommerce.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Commands
{
    internal class CommandDispatcher : ICommandDispatcher   
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            if (command is null)
            {
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }

        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command)
        {
            if (command is null)
            {
                throw new ArgumentException("Command cannot be null.");
            }

            using var scope = _serviceProvider.CreateScope();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var result = await (Task<TResult>)handlerType
                .GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))
                ?.Invoke(handler, new[] { command });

            return result;
        }
    }
}
