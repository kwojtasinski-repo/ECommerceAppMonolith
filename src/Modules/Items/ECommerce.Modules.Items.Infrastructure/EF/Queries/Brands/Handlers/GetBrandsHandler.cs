using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Brands;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Brands.Handlers
{
    internal class GetBrandsHandler : IQueryHandler<GetBrands, IEnumerable<BrandDto>>
    {
        private readonly DbSet<Brand> _brands;

        public GetBrandsHandler(ItemsDbContext itemsDbContext)
        {
            _brands = itemsDbContext.Brands;
        }

        public async Task<IEnumerable<BrandDto>> HandleAsync(GetBrands query)
        {
            var brands = await _brands.AsNoTracking().ToListAsync();
            return brands.Select(b => b.AsDto());
        }
    }
}
