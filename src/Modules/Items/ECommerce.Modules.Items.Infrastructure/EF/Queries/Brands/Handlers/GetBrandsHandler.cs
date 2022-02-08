using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Queries.Brands;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Brands.Handlers
{
    internal class GetBrandsHandler : IQueryHandler<GetBrands, IEnumerable<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandsHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IEnumerable<BrandDto>> HandleAsync(GetBrands query)
        {
            var brands = await _brandRepository.GetAllAsync();
            return brands.Select(b => b.AsDto());
        }
    }
}
