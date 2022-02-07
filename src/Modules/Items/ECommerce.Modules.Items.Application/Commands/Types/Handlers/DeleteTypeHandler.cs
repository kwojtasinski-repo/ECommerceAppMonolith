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
    internal class DeleteTypeHandler : ICommandHandler<DeleteType>
    {
        private readonly ITypeRepository _typeRepository;

        public DeleteTypeHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task HandleAsync(DeleteType command)
        {
            var type = await _typeRepository.GetDetailsAsync(command.TypeId);

            if (type is null)
            {
                throw new TypeNotFoundException(command.TypeId);
            }

            if(type.Items is not null && type.Items.Any())
            {
                throw new TypeCannotBeDeletedException(type.Id, type.Name);
            }

            await _typeRepository.DeleteAsync(type);
        }
    }
}
