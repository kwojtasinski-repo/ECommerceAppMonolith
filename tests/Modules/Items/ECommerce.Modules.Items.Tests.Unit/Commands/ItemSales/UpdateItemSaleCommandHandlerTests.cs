using ECommerce.Modules.Items.Application.Commands.ItemSales;
using ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers;
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

namespace ECommerce.Modules.Items.Tests.Unit.Commands.ItemSales
{
    public class UpdateItemSaleCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_update()
        {
            var command = new UpdateItemSale(Guid.NewGuid(), 1500M);
            var itemSale = CreateItemSale(command.ItemSaleId);
            _itemSaleRepository.GetAsync(command.ItemSaleId).Returns(itemSale);

            await _handler.HandleAsync(command);

            await _itemSaleRepository.Received(1).UpdateAsync(Arg.Any<ItemSale>());
            _eventMapper.MapAll(Arg.Any<IEnumerable<IDomainEvent>>()).Received(1);
            await _messageBroker.Received(1).PublishAsync(Arg.Any<IMessage[]>());
        }

        [Fact]
        public async Task given_null_command_should_throw_an_exception()
        {
            var expectedException = new UpdateItemSaleCannotBeNullException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UpdateItemSaleCannotBeNullException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_negative_cost_should_throw_an_exception()
        {
            var command = new UpdateItemSale(Guid.NewGuid(), -1500M);
            var expectedException = new ItemCostCannotBeNegativeException(command.ItemCost);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemCostCannotBeNegativeException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemCostCannotBeNegativeException)exception).ItemCost.ShouldBe(expectedException.ItemCost);
        }

        [Fact]
        public async Task given_invalid_item_sale_id_should_throw_an_exception()
        {
            var command = new UpdateItemSale(Guid.NewGuid(), 1500M);
            var expectedException = new ItemSaleNotFoundException(command.ItemSaleId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemSaleNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemSaleNotFoundException)exception).ItemSaleId.ShouldBe(expectedException.ItemSaleId);
        }

        private readonly UpdateItemSaleHandler _handler;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public UpdateItemSaleCommandHandlerTests()
        {
            _itemSaleRepository = Substitute.For<IItemSaleRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _handler = new UpdateItemSaleHandler(_itemSaleRepository, _messageBroker, _eventMapper);
        }

        private ItemSale CreateItemSale(Guid id)
        {
            return new ItemSale(id, CreateSampleItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), 1250M, true);
        }

        private Domain.Entities.Item CreateSampleItem(Guid id, Guid brandId, Guid typeId)
        {
            var item = new Domain.Entities.Item(id, "Item #1", CreateSampleBrand(brandId), CreateSampleType(typeId),
                "descrption", null, null);
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
