using ECommerce.Modules.Items.Domain.Entities;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    public interface IItemSaleRepository
    {
        Task<IReadOnlyList<ItemSale>> GetAllAsync();
        Task<ItemSale?> GetAsync(Guid id);
        Task AddAsync(ItemSale itemSale);
        Task UpdateAsync(ItemSale itemSale);
        Task DeleteAsync(ItemSale itemSale);
    }
}
