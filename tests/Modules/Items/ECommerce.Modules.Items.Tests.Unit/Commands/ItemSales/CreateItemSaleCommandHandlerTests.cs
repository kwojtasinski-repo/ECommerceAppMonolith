using ECommerce.Modules.Items.Application.Commands.ItemsSale;
using ECommerce.Modules.Items.Application.Commands.ItemsSale.Handlers;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Entities.ValueObjects;
using ECommerce.Modules.Items.Domain.Repositories;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.ItemSales
{
    public class CreateItemSaleCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_add()
        {
            var command = new CreateItemSale(Guid.NewGuid(), 1250M);
            var dictionary = new Dictionary<string, IEnumerable<ItemImage>>();
            dictionary.Add(Item.IMAGES, new[] { new ItemImage { Url = "http://google.pl", MainImage = true } });
            var item = CreateSampleItem(command.ItemId, Guid.NewGuid(), Guid.NewGuid(), dictionary);
            _itemRepository.GetAsync(command.ItemId).Returns(item);

            await _handler.HandleAsync(command);

            await _itemSaleRepository.Received(1).AddAsync(Arg.Any<ItemSale>());
        }

        [Fact]
        public async Task given_item_without_main_image_should_throw_an_exception()
        {
            var command = new CreateItemSale(Guid.NewGuid(), 1250M);
            var dictionary = new Dictionary<string, IEnumerable<ItemImage>>();
            dictionary.Add(Item.IMAGES, new[] { new ItemImage { Url = "http://google.pl", MainImage = false } });
            var item = CreateSampleItem(command.ItemId, Guid.NewGuid(), Guid.NewGuid(), dictionary);
            _itemRepository.GetAsync(command.ItemId).Returns(item);
            var expectedException = new CannotCreateItemSaleWithoutMainImageException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotCreateItemSaleWithoutMainImageException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_item_without_images_should_throw_an_exception()
        {
            var command = new CreateItemSale(Guid.NewGuid(), 1250M);
            var item = CreateSampleItem(command.ItemId, Guid.NewGuid(), Guid.NewGuid(), null);
            _itemRepository.GetAsync(command.ItemId).Returns(item);
            var expectedException = new CannotCreateItemSaleWithoutImagesException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotCreateItemSaleWithoutImagesException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_item_id_should_throw_an_exception()
        {
            var command = new CreateItemSale(Guid.NewGuid(), 1250M);
            var item = CreateSampleItem(command.ItemId, Guid.NewGuid(), Guid.NewGuid(), null);
            var expectedException = new ItemNotFoundException(item.Id);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemNotFoundException)exception).ItemId.ShouldBe(expectedException.ItemId);
        }

        [Fact]
        public async Task given_null_command_should_throw_an_exception()
        {
            var expectedException = new CreateItemSaleCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CreateItemSaleCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_negative_cost_should_throw_an_exception()
        {
            var command = new CreateItemSale(Guid.NewGuid(), -1500M);
            var expectedException = new ItemCostCannotBeNegativeException(command.ItemCost);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemCostCannotBeNegativeException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemCostCannotBeNegativeException)exception).ItemCost.ShouldBe(expectedException.ItemCost);
        }

        private readonly CreateItemSaleHandler _handler;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemRepository _itemRepository;

        public CreateItemSaleCommandHandlerTests()
        {
            _itemSaleRepository = Substitute.For<IItemSaleRepository>();
            _itemRepository = Substitute.For<IItemRepository>();
            _handler = new CreateItemSaleHandler(_itemSaleRepository, _itemRepository);
        }

        private Domain.Entities.Item CreateSampleItem(Guid id, Guid brandId, Guid typeId, Dictionary<string, IEnumerable<ItemImage>> images)
        {
            var item = new Domain.Entities.Item(id, "Item #1", CreateSampleBrand(brandId), CreateSampleType(typeId),
                "descrption", null, images);
            return item;
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
