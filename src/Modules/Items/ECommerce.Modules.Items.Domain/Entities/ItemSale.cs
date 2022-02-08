using ECommerce.Modules.Items.Domain.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    public class ItemSale : AggregateRoot
    {
        public Item Item { get; private set; }
        public AggregateId ItemId { get; private set; }
        public decimal Cost { get; private set; }
        public bool? Active { get; private set; }

        public ItemSale(AggregateId id, Item item, decimal cost, bool active, int version = 0)
        {
            ValidItem(item);
            Id = id;
            Item = item;
            ItemId = item.Id;
            Cost = cost;
            Active = active;
            Version = version;
        }

        private ItemSale(AggregateId id)
        {
            Id = id;
        }

        public static ItemSale Create(AggregateId id, Item item, decimal cost)
        {
            var itemSale = new ItemSale(id);
            ValidItem(item);
            itemSale.Item = item;
            itemSale.ChangeCost(cost);
            itemSale.ChangeActive(true);

            itemSale.ClearEvents();
            itemSale.Version = 0;

            return itemSale;
        }

        public void ChangeCost(decimal cost)
        {
            Cost = cost;
            IncrementVersion();
        }

        public void ChangeActive(bool active)
        {
            Active = active;
            IncrementVersion();
        }

        private static void ValidItem(Item item)
        {
            if (item is null)
            {
                throw new ItemCannotBeNullException();
            }
        }
    }
}
