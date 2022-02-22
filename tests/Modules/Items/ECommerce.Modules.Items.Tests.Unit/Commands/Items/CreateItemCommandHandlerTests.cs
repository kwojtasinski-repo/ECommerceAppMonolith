using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.Commands.Items.Handlers;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.Items
{
    public class CreateItemCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_item_should_add()
        {
            var command = new CreateItem("item #1", "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            _brandRepository.GetAsync(command.BrandId).Returns(CreateSampleBrand(command.BrandId));
            _typeRepository.GetAsync(command.TypeId).Returns(CreateSampleType(command.TypeId));

            await _handler.HandleAsync(command);

            await _itemRepository.Received(1).AddAsync(Arg.Any<Item>());
            _eventMapper.MapAll(Arg.Any<IEnumerable<IDomainEvent>>()).Received(1);
            await _messageBroker.Received(1).PublishAsync(Arg.Any<IMessage[]>());
        }

        [Fact]
        public async Task given_null_command_should_throw_an_exception()
        {
            var expectedException = new CreateItemCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CreateItemCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_empty_item_name_should_throw_an_exception()
        {
            var command = new CreateItem("", "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            var expectedException = new ItemNameCannotBeEmptyException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemNameCannotBeEmptyException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_too_short_name_should_throw_an_exception()
        {
            var command = new CreateItem("a", "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            var expectedException = new ItemNameTooShortException(command.ItemName);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemNameTooShortException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemNameTooShortException)exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_too_long_name_should_throw_an_exception()
        {
            var command = new CreateItem("abcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyz",
                "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            var expectedException = new ItemNameTooLongException(command.ItemName);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemNameTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemNameTooLongException)exception).Name.ShouldBe(expectedException.Name);
        }

        [Fact]
        public async Task given_two_main_images_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(), null, 
                new[] { new ImageUrl { Url = "http://google.pl", MainImage = true }, new ImageUrl { Url = "http://google.pl", MainImage = true } });
            var expectedException = new ItemImagesNotAllowedMoreThanOneMainImageException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemImagesNotAllowedMoreThanOneMainImageException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_images_url_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(), null,
                new[] { new ImageUrl { Url = "", MainImage = false }, new ImageUrl { Url = "", MainImage = false } });
            var expectedException = new UrlsCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UrlsCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_brand_id_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            var expectedException = new BrandNotFoundException(command.BrandId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BrandNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((BrandNotFoundException)exception).Id.ShouldBe(expectedException.Id);
        }

        [Fact]
        public async Task given_invalid_type_id_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(), null, null);
            var expectedException = new TypeNotFoundException(command.TypeId);
            _brandRepository.GetAsync(command.BrandId).Returns(CreateSampleBrand(command.BrandId));

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TypeNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TypeNotFoundException)exception).Id.ShouldBe(expectedException.Id);
        }

        [Fact]
        public async Task given_tags_with_empty_values_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(),
                new[] { "", ""}, null);
            var expectedException = new TagsCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TagsCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_too_long_tags_should_throw_an_exception()
        {
            var command = new CreateItem("test #1", "description", Guid.NewGuid(), Guid.NewGuid(), 
                new[] { "abcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyz1234",
                    "abcdefghijklmnoprstuvwxyzabcdefghijklmnoprstuvwxyz1234"}, null);
            var expectedException = new TagsTooLongException(command.Tags);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TagsTooLongException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((TagsTooLongException)exception).Tags.ShouldBe(expectedException.Tags);
        }

        private readonly CreateItemHandler _handler;
        private readonly IItemRepository _itemRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public CreateItemCommandHandlerTests()
        {
            _itemRepository = Substitute.For<IItemRepository>();
            _typeRepository = Substitute.For<ITypeRepository>();
            _brandRepository = Substitute.For<IBrandRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _handler = new CreateItemHandler(_itemRepository, _typeRepository, _brandRepository, _messageBroker, _eventMapper);
        }

        private Domain.Entities.Brand CreateSampleBrand(Guid? id)
        {
            if (id == null)
            {
                id = Guid.NewGuid();
            }

            var brand = new Domain.Entities.Brand(id, "Brand #1");
            return brand;
        }

        private Domain.Entities.Type CreateSampleType(Guid? id)
        {
            if (id == null)
            {
                id = Guid.NewGuid();
            }

            var type = new Domain.Entities.Type(id, "Type #1");
            return type;
        }
    }
}
