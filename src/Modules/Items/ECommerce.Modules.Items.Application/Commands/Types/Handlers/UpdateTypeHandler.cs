using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Types.Handlers
{
    internal class UpdateTypeHandler : ICommandHandler<UpdateType>
    {
        private readonly ITypeRepository _typeRepository;

        public UpdateTypeHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task HandleAsync(UpdateType command)
        {
            Validate(command);
            var type = await _typeRepository.GetAsync(command.TypeId);

            if (type is null)
            {
                throw new TypeNotFoundException(command.TypeId);
            }

            if (type.Name == command.Name)
            {
                return;
            }

            type.ChangeName(command.Name);
            await _typeRepository.UpdateAsync(type);
        }

        private static void Validate(UpdateType command)
        {
            if (command is null)
            {
                throw new UpdateTypeCannotBeNullException();
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
