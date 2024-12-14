using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Users.Core.DAL.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetAsync(Guid id)
        {
            var user = await _context.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetAsync(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<IEnumerable<User>> GetAllByEmailAsync(string email)
        {
            var users = await _context.Users
                            .Where(u => u.Email.StartsWith(email) || u.Email.EndsWith(email))
                            .ToListAsync();
            return users;
        }

        public async Task<User?> GetUserDataAsync(Guid id)
        {
            return await _context.Users
                                    .Select(u => new User {
                                        Id = u.Id,
                                        Email = u.Email,
                                        CreatedAt = u.CreatedAt,
                                        IsActive = u.IsActive,
                                    })
                                    .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
