using ECommerce.Modules.Users.Core.Entities;
using ECommerce.Modules.Users.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<User> GetAsync(Guid id)
        {
            var user = await _context.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _context.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
