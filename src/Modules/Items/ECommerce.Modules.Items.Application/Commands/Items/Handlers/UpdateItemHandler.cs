using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Policies.Items;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class UpdateItemHandler : ICommandHandler<UpdateItem>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IItemUpdatePolicy _itemUpdatePolicy;

        public UpdateItemHandler(IItemRepository itemRepository, ITypeRepository typeRepository, IBrandRepository brandRepository, IItemUpdatePolicy itemUpdatePolicy)
        {
            _itemRepository = itemRepository;
            _typeRepository = typeRepository;
            _brandRepository = brandRepository;
            _itemUpdatePolicy = itemUpdatePolicy;
        }

        public async Task HandleAsync(UpdateItem command)
        {
            Validate(command);
            var brand = await _brandRepository.GetAsync(command.BrandId);

            if (brand is null)
            {
                throw new BrandNotFoundException(command.BrandId);
            }

            var type = await _typeRepository.GetAsync(command.TypeId);

            if (type is null)
            {
                throw new TypeNotFoundException(command.TypeId);
            }

            var item = await _itemRepository.GetAsync(command.ItemId);

            if (item is null)
            {
                throw new ItemNotFoundException(command.ItemId);
            }

            if (await _itemUpdatePolicy.CanUpdateAsync(item) is false)
            {
                throw new CannotUpdateItemException(item.Id);
            }

            item.ChangeName(command.ItemName);
            item.ChangeDescription(command.Description);
            item.ChangeBrand(brand);
            item.ChangeType(type);
            item.ChangeTags(command.Tags);
            var urls = command.ImagesUrl.ToImageDictionary();
            item.ChangeImagesUrl(urls);
            
            await _itemRepository.UpdateAsync(item);
        }

        private static void Validate(UpdateItem command)
        {
            if (command is null)
            {
                throw new UpdateItemCannotBeNullException();
            }

            if (string.IsNullOrWhiteSpace(command.ItemName))
            {
                throw new ItemNameCannotBeEmptyException();
            }

            if (command.ItemName.Length < 3)
            {
                throw new ItemNameTooShortException(command.ItemName);
            }

            if (command.ItemName.Length > 100)
            {
                throw new ItemNameTooLongException(command.ItemName);
            }

            if (command.ImagesUrl is not null)
            {
                var mainImages = command.ImagesUrl.Where(im => im.MainImage).Count();

                if (mainImages > 1)
                {
                    throw new ItemImagesNotAllowedMoreThanOneMainImageException();
                }
            }
        }
    }
}
