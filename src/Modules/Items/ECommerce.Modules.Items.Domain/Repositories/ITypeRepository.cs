using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    public interface ITypeRepository
    {
        Task<IReadOnlyList<Entities.Type>> GetAllAsync();
        Task<bool> ExistsAsync(Guid id);
        Task<Entities.Type> GetAsync(Guid id);
        Task<Entities.Type> GetDetailsAsync(Guid id);
        Task AddAsync(Entities.Type type);
        Task UpdateAsync(Entities.Type type);
        Task DeleteAsync(Entities.Type type);
    }
}
