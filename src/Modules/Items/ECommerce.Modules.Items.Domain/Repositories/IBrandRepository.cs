using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    internal interface IBrandRepository
    {
        Task<IReadOnlyList<Brand>> GetAllAsync();
        Task<Brand> GetAsync(Guid id);
        Task<Brand> GetDetailsAsync(Guid id);
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Brand brand);
    }
}
