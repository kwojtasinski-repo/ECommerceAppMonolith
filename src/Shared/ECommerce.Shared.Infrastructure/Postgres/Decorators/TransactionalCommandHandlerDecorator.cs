using ECommerce.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.Postgres.Decorators
{
    [Decorator]
    internal class TransactionalCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UnitOfWorkTypeRegistry _unitOfWorkTypeRegistry;
        private readonly ICommandHandler<T> _handler;

        public TransactionalCommandHandlerDecorator(IServiceProvider serviceProvider, UnitOfWorkTypeRegistry unitOfWorkTypeRegistry, ICommandHandler<T> handler)
        {
            _serviceProvider = serviceProvider;
            _unitOfWorkTypeRegistry = unitOfWorkTypeRegistry;
            _handler = handler;
        }

        public async Task HandleAsync(T command)
        {
            var unitOfWorkType = _unitOfWorkTypeRegistry.Resolve<T>();

            if (unitOfWorkType is null)
            {
                await _handler.HandleAsync(command);
                return;
            }

            var unitOfWork = (IUnitOfWork)_serviceProvider.GetRequiredService(unitOfWorkType);
            await unitOfWork.ExecuteAsync(() => _handler.HandleAsync(command));
        }
    }
}
