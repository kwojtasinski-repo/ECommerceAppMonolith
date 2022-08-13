using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Policies.Items
{
    internal class ItemUpdatePolicy : IItemUpdatePolicy
    {
        public Task<bool> CanUpdateAsync(Item item)
        {
            bool canUpdate = true;

            if (item.ItemSale is not null)
            {
                canUpdate = item.ItemSale.Active == false;
            }

            return Task.FromResult(canUpdate);
        }
    }
}
