using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Policies.Items
{
    internal class ItemDeletionPolicy : IItemDeletionPolicy
    {
        public Task<bool> CanDeleteAsync(Item item)
        {
            bool canDelete = true;

            if (item.ItemSale is not null)
            {
                canDelete = false;
            }

            return Task.FromResult(canDelete);
        }
    }
}
