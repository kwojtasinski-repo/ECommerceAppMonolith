using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Types;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Types.Handlers
{
    internal class GetTypesHandler : IQueryHandler<GetTypes, IEnumerable<TypeDto>>
    {
        private readonly ITypeRepository _typeRepository;

        public GetTypesHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IEnumerable<TypeDto>> HandleAsync(GetTypes query)
        {
            var types = await _typeRepository.GetAllAsync();
            return types.Select(t => t.AsDto());
        }
    }
}
