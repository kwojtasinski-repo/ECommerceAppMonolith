using ECommerce.Modules.Items.Domain.Events;
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
        public string CurrencyCode { get; private set; }
        public bool? Active { get; private set; }

        public ItemSale(AggregateId id, Item item, decimal cost, bool active, string currencyCode, int version = 0)
        {
            ValidItem(item);
            Id = id;
            Item = item;
            ItemId = item.Id;
            Cost = cost;
            CurrencyCode = currencyCode;
            Active = active;
            Version = version;
        }

        private ItemSale(AggregateId id)
        {
            Id = id;
        }

        public static ItemSale Create(AggregateId id, Item item, decimal cost, string currencyCode)
        {
            var itemSale = new ItemSale(id);
            ValidItem(item);
            itemSale.Item = item;
            itemSale.ChangeCost(cost);
            itemSale.ChangeActive(true);
            itemSale.ChangeCurrencyCode(currencyCode);
            itemSale.ClearEvents();
            itemSale.Version = 0;
            itemSale.AddEvent(new ItemSaleCreated(itemSale));

            return itemSale;
        }

        public void ChangeCost(decimal cost)
        {
            if (cost < 0)
            {
                throw new CostCannotBeNegativeException(cost);
            }

            Cost = cost;
            IncrementVersion();
            AddEvent(new ItemSaleCostChanged(this));
        }

        public void ChangeActive(bool active)
        {
            Active = active;
            IncrementVersion();
        }

        public void ChangeCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new CurrencyCodeCannotBeNullException();
            }
            
            CurrencyCode = currencyCode;
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
