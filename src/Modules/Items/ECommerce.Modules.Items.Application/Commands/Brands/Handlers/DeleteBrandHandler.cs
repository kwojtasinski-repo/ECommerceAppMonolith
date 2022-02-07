using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Brands.Handlers
{
    internal class DeleteBrandHandler : ICommandHandler<DeleteBrand>
    {
        private readonly IBrandRepository _brandRepository;

        public DeleteBrandHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task HandleAsync(DeleteBrand command)
        {
            var brand = await _brandRepository.GetDetailsAsync(command.BrandId);

            if (brand is null)
            {
                throw new BrandNotFoundException(command.BrandId);
            }

            if (brand.Items is not null && brand.Items.Any())
            {
                throw new BrandCannotBeDeletedException(brand.Id, brand.Name);
            }

            await _brandRepository.DeleteAsync(brand);
        }
    }
}
