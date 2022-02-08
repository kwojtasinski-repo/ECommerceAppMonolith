using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class ItemCostCannotBeNegativeException : ECommerceException
    {
        public decimal ItemCost { get; }

        public ItemCostCannotBeNegativeException(decimal itemCost) : base($"ItemCost '{itemCost}' cannot be negative.")
        {
            ItemCost = itemCost;
        }
    }
}
