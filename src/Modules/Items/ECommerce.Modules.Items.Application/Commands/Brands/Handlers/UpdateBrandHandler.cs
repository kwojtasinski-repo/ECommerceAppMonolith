using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Brands.Handlers
{
    internal class UpdateBrandHandler : ICommandHandler<UpdateBrand>
    {
        private readonly IBrandRepository _brandRepository;

        public UpdateBrandHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task HandleAsync(UpdateBrand command)
        {
            Validate(command);
            var brand = await _brandRepository.GetAsync(command.BrandId);
            
            if (brand is null)
            {
                throw new BrandNotFoundException(command.BrandId);
            }

            if (brand.Name == command.Name)
            {
                return;
            }

            brand.ChangeName(command.Name);
            await _brandRepository.UpdateAsync(brand);
        }

        private static void Validate(UpdateBrand command)
        {
            if (command is null)
            {
                throw new UpdateBrandCannotBeNullException();
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new InvalidBrandNameException();
            }

            if (command.Name.Length < 3)
            {
                throw new BrandNameTooShortException(command.Name);
            }

            if (command.Name.Length > 100)
            {
                throw new BrandNameTooLongException(command.Name);
            }
        }
    }
}
