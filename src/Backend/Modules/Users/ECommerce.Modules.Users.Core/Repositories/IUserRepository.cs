using ECommerce.Modules.Users.Core.Entities;

namespace ECommerce.Modules.Users.Core.Repositories
{
    internal interface IUserRepository
    {
        Task<User?> GetAsync(Guid id);
        Task<User?> GetUserDataAsync(Guid id);
        Task<User?> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
