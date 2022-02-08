using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Types;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Types.Handlers
{
    internal class GetTypeHandler : IQueryHandler<GetType, TypeDto>
    {
        private readonly ITypeRepository _typeRepository;

        public GetTypeHandler(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<TypeDto> HandleAsync(GetType query)
        {
            var type = await _typeRepository.GetAsync(query.TypeId);
            return type.AsDto();
        }
    }
}
