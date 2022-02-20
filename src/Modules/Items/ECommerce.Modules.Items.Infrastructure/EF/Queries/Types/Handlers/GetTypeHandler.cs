using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Types;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Types.Handlers
{
    internal class GetTypeHandler : IQueryHandler<GetType, TypeDto>
    {
        private readonly DbSet<Domain.Entities.Type> _types;

        public GetTypeHandler(ItemsDbContext itemsDbContext)
        {
            _types = itemsDbContext.Types;
        }

        public async Task<TypeDto> HandleAsync(GetType query)
        {
            var type = await _types.Where(t => t.Id == query.TypeId).AsNoTracking().SingleOrDefaultAsync();
            return type?.AsDto();
        }
    }
}
