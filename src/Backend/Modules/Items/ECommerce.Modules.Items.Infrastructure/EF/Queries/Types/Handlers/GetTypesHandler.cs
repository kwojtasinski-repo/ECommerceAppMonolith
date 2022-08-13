using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Types;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Types.Handlers
{
    internal class GetTypesHandler : IQueryHandler<GetTypes, IEnumerable<TypeDto>>
    {
        private readonly DbSet<Domain.Entities.Type> _types;

        public GetTypesHandler(ItemsDbContext itemsDbContext)
        {
            _types = itemsDbContext.Types;
        }

        public async Task<IEnumerable<TypeDto>> HandleAsync(GetTypes query)
        {
            var types = await _types.ToListAsync();
            return types.Select(t => t.AsDto());
        }
    }
}
