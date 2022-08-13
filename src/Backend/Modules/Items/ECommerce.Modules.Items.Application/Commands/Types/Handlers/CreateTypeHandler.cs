using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Types.Handlers
{
    internal class CreateTypeHandler : ICommandHandler<CreateType>
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public CreateTypeHandler(ITypeRepository typeRepository, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _typeRepository = typeRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(CreateType command)
        {
            Validate(command);
            var entity = Domain.Entities.Type.Create(command.TypeId, command.Name);
            await _typeRepository.AddAsync(entity);

            var integrationEvents = _eventMapper.MapAll(entity.Events);
            await _messageBroker.PublishAsync(integrationEvents.ToArray());
        }

        private static void Validate(CreateType command)
        {
            if (command is null)
            {
                throw new CreateTypeCannotBeNullException();
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new InvalidTypeNameException();
            }

            if (command.Name.Length < 3)
            {
                throw new TypeNameTooShortException(command.Name);
            }

            if (command.Name.Length > 100)
            {
                throw new TypeNameTooLongException(command.Name);
            }
        }
    }
}
