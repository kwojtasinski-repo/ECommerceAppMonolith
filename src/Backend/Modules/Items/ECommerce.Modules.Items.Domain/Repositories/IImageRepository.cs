using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    public interface IImageRepository
    {
        Task<IReadOnlyList<Image>> GetAllAsync();
        Task<Image> GetAsync(Guid id);
        Task AddAsync(Image image);
        Task UpdateAsync(Image image);
        Task DeleteAsync(Image image);
    }
}
