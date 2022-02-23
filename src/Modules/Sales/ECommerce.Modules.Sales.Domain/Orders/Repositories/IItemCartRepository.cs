using ECommerce.Modules.Sales.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Repositories
{
    public interface IItemCartRepository
    {
        Task<ItemCart> GetAsync(Guid id);
    }
}
