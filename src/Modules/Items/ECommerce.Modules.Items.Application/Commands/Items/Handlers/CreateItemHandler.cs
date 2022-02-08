﻿using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class CreateItemHandler : ICommandHandler<CreateItem>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IBrandRepository _brandRepository;

        public CreateItemHandler(IItemRepository itemRepository, ITypeRepository typeRepository, IBrandRepository brandRepository)
        {
            _itemRepository = itemRepository;
            _typeRepository = typeRepository;
            _brandRepository = brandRepository;
        }

        public async Task HandleAsync(CreateItem command)
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

            var urls = command.ImagesUrl.ToImageDictionary();
            var item = Item.Create(command.ItemId, command.ItemName, brand, type, command.Description,
                                    command.Tags, urls);
            await _itemRepository.AddAsync(item);
        }

        private static void Validate(CreateItem command)
        {
            if (command is null)
            {
                throw new CreateItemCannotBeNullException();
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