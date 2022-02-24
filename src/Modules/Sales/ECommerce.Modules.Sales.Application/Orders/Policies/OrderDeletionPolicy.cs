using ECommerce.Modules.Sales.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Policies
{
    internal class OrderDeletionPolicy : IOrderDeletionPolicy
    {
        public Task<bool> CanDeleteAsync(Order order)
        {
            var canDelete = true;

            if(order.OrderItems is not null && order.OrderItems.Any())
            {
                canDelete = false;
            }

            if (order.Paid is true)
            {
                canDelete = false;
            }

            return Task.FromResult(canDelete);
        }
    }
}
