using ECommerce.Modules.Items.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Repositories
{
    internal interface IImageRepository
    {
        Task<Image> GetAllAsync();
        Task<Image> GetAllAsync(Guid itemId);
        Task<Image> GetAsync(Guid id);
        Task AddAsync(Image image);
        Task UpdateAsync(Image image);
        Task DeleteAsync(Image image);
    }
}
