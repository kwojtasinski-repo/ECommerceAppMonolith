using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Brands;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Brands.Handlers
{
    internal class GetBrandHandler : IQueryHandler<GetBrand, BrandDto>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<BrandDto> HandleAsync(GetBrand query)
        {
            var brand = await _brandRepository.GetAsync(query.BrandId);
            return brand.AsDto();
        }
    }
}
