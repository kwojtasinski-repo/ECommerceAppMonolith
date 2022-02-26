using ECommerce.Modules.Sales.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Policies
{
    internal class OrderPositionModificationPolicy : IOrderPositionModificationPolicy
    {
        public Task<bool> CanAddAsync(Order order)
        {
            var canAdd = true;

            if (order.Paid is true)
            {
                canAdd = false;
            }
            
            return Task.FromResult(canAdd);
        }

        public Task<bool> CanDeleteAsync(Order order)
        {
            var canDelete = true;

            if (order.Paid is true)
            {
                canDelete = false;
            }

            return Task.FromResult(canDelete);
        }
    }
}
