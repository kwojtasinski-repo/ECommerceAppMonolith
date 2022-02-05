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
        public Guid ItemId { get; private set; }
        public decimal Cost { get; private set; }
        public bool Active { get; private set; }

        public ItemSale(AggregateId id, Item item, decimal cost, bool active = true)
        {
            Id = id;
            Item = item;
            ItemId = item.Id;
            Cost = cost;
            Active = active;
        }

        private ItemSale(AggregateId id, Item item)
        {
            Id = id;
            Item = item;
        }

        public static ItemSale Create(AggregateId id, Item item, decimal cost)
        {
            var itemSale = new ItemSale(id, item);
            itemSale.ChangeCost(cost);
            itemSale.Active = true;

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
    }
}
