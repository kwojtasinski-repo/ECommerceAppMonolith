using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Brands;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Brands.Handlers
{
    internal class GetBrandHandler : IQueryHandler<GetBrand, BrandDto>
    {
        private readonly DbSet<Brand> _brands;

        public GetBrandHandler(ItemsDbContext itemsDbContext)
        {
            _brands = itemsDbContext.Brands;
        }

        public async Task<BrandDto> HandleAsync(GetBrand query)
        {
            var brand = await _brands.Where(b => b.Id == query.BrandId).AsNoTracking().SingleOrDefaultAsync();
            return brand.AsDto();
        }
    }
}
