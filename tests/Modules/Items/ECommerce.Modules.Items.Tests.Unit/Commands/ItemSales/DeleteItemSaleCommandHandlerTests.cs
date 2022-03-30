using ECommerce.Modules.Items.Application.Commands.ItemSales;
using ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
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
    public class DeleteItemSaleCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_delete()
        {
            var command = new DeleteItemSale(Guid.NewGuid());
            var itemSale = CreateItemSale(command.ItemSaleId);
            _itemSaleRepository.GetAsync(command.ItemSaleId).Returns(itemSale);

            await _handler.HandleAsync(command);

            await _itemSaleRepository.Received(1).DeleteAsync(itemSale);
        }

        [Fact]
        public async Task given_invalid_item_sale_id_should_throw_an_exception()
        {
            var command = new DeleteItemSale(Guid.NewGuid());
            var expectedException = new ItemSaleNotFoundException(command.ItemSaleId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemSaleNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemSaleNotFoundException)exception).ItemSaleId.ShouldBe(expectedException.ItemSaleId);
        }

        private readonly DeleteItemSaleHandler _handler;
        private readonly IItemSaleRepository _itemSaleRepository;

        public DeleteItemSaleCommandHandlerTests()
        {
            _itemSaleRepository = Substitute.For<IItemSaleRepository>();
            _handler = new DeleteItemSaleHandler(_itemSaleRepository);
        }

        private ItemSale CreateItemSale(Guid id)
        {
            return new ItemSale(id, CreateSampleItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), 1250M, true, "PLN");
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
