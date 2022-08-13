using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Repositories
{
    internal class ItemCartRepository : IItemCartRepository
    {
        private readonly SalesDbContext _salesDbContext;

        public ItemCartRepository(SalesDbContext salesDbContext)
        {
            _salesDbContext = salesDbContext;
        }

        public async Task AddAsync(ItemCart itemCart)
        {
            await _salesDbContext.ItemCarts.AddAsync(itemCart);
            await _salesDbContext.SaveChangesAsync();
        }

        public Task<ItemCart> GetAsync(Guid id)
        {
            var itemCart = _salesDbContext.ItemCarts.Where(i => i.Id == id).SingleOrDefaultAsync();
            return itemCart;
        }
    }
}
