using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Shared.Abstractions.Messagging;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class CreateItemHandler : ICommandHandler<CreateItem>
    {
        private readonly IItemRepository _itemRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public CreateItemHandler(IItemRepository itemRepository, ITypeRepository typeRepository, IBrandRepository brandRepository, 
            IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _itemRepository = itemRepository;
            _typeRepository = typeRepository;
            _brandRepository = brandRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
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

            var integrationEvents = _eventMapper.MapAll(item.Events);
            await _messageBroker.PublishAsync(integrationEvents.ToArray());
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

            if (command.Tags is not null)
            {
                var emptyTags = command.Tags.Where(t => string.IsNullOrWhiteSpace(t));

                if (emptyTags.Any())
                {
                    throw new TagsCannotBeNullException();
                }

                var tooLongTags = command.Tags.Where(t => t.Length > 50);

                if (tooLongTags.Any())
                {
                    throw new TagsTooLongException(tooLongTags);
                }
            }

            if (command.ImagesUrl is not null)
            {
                var emptyUrls = command.ImagesUrl.Where(im => string.IsNullOrWhiteSpace(im.Url));

                if (emptyUrls.Any())
                {
                    throw new UrlsCannotBeNullException();
                }

                var mainImages = command.ImagesUrl.Where(im => im.MainImage).Count();

                if (mainImages > 1)
                {
                    throw new ItemImagesNotAllowedMoreThanOneMainImageException();
                }
            }
        }
    }
}
