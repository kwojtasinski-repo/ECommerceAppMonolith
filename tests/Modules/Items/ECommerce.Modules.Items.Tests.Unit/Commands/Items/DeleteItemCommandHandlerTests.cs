using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.Commands.Items.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Policies.Items;
using ECommerce.Modules.Items.Domain.Repositories;
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
    public class DeleteItemCommandHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_delete()
        {
            var item = CreateSampleItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var command = new DeleteItem(item.Id);
            _itemRepository.GetAsync(item.Id).Returns(item);
            _itemDeletionPolicy.CanDeleteAsync(item).Returns(true);

            await _handler.HandleAsync(command);

            await _itemRepository.Received(1).DeleteAsync(item);
        }

        [Fact]
        public async Task given_invali_item_id_should_throw_an_exception()
        {
            var item = CreateSampleItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var command = new DeleteItem(item.Id);
            var expectedException = new ItemNotFoundException(command.ItemId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ItemNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((ItemNotFoundException)exception).ItemId.ShouldBe(expectedException.ItemId);
        }

        [Fact]
        public async Task given_item_with_policy_cannot_delete_should_throw_an_exception()
        {
            var item = CreateSampleItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var command = new DeleteItem(item.Id);
            _itemRepository.GetAsync(item.Id).Returns(item);
            var expectedException = new CannotDeleteItemException(command.ItemId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotDeleteItemException>();
            exception.Message.ShouldBe(expectedException.Message);
            ((CannotDeleteItemException)exception).ItemId.ShouldBe(expectedException.ItemId);
        }

        private readonly DeleteItemHandler _handler;
        private readonly IItemRepository _itemRepository;
        private readonly IItemDeletionPolicy _itemDeletionPolicy;

        public DeleteItemCommandHandlerTests()
        {
            _itemRepository = Substitute.For<IItemRepository>();
            _itemDeletionPolicy = Substitute.For<IItemDeletionPolicy>();
            _handler = new DeleteItemHandler(_itemRepository, _itemDeletionPolicy);
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
