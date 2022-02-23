using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class ItemCartRepository : IItemCartRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public ItemCartRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public Task<ItemCart> GetAsync(Guid id)
        {
            var itemCart = _salesDbContext.ItemCarts.Where(i => i.Id == id).SingleOrDefaultAsync();
            return itemCart;
        }
    }
}
