using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
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

        public CreateTypeHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task HandleAsync(CreateType command)
        {
            Validate(command);
            var entity = Domain.Entities.Type.Create(command.Id, command.Name);
            await _typeRepository.AddAsync(entity);
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
