using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Policies.Items
{
    public interface IItemUpdatePolicy
    {
        Task<bool> CanUpdateAsync(Item item);
    }
}
