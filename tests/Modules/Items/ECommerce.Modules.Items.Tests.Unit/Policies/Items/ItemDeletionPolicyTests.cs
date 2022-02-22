using ECommerce.Modules.Items.Application.Policies.Items;
using ECommerce.Modules.Items.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Policies.Items
{
    public class ItemDeletionPolicyTests
    {
        [Fact]
        public async Task given_valid_item_should_allow_deletion()
        {
            var item = new Item(Guid.NewGuid(), "Test", new Brand(Guid.NewGuid(), "TestBrand"), new Domain.Entities.Type(Guid.NewGuid(), "TestType"),
                                "Description", null, null);

            var canDelete = await _itemDeletionPolicy.CanDeleteAsync(item);

            canDelete.ShouldBeTrue();
        }

        [Fact]
        public async Task given_item_with_item_sale_shouldnt_allow_deletion()
        {
            var item = new Item(Guid.NewGuid(), "Test", new Brand(Guid.NewGuid(), "TestBrand"), new Domain.Entities.Type(Guid.NewGuid(), "TestType"),
                                "Description", null, null);
            var itemSale = ItemSale.Create(Guid.NewGuid(), item, 1500M);
            item.ChangeItemSale(itemSale);

            var canDelete = await _itemDeletionPolicy.CanDeleteAsync(item);

            canDelete.ShouldBeFalse();
        }

        private readonly IItemDeletionPolicy _itemDeletionPolicy;

        public ItemDeletionPolicyTests()
        {
            _itemDeletionPolicy = new ItemDeletionPolicy();
        }
    }
}
