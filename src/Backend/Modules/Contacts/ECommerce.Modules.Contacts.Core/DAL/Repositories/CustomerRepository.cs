using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Modules.Contacts.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DAL.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ContactsDbContext _context;

        public CustomerRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<Customer>> GetAllByUserIdAsync(Guid userId)
        {
            var customers = await _context.Customers.Where(c => c.UserId == userId && c.Active).AsNoTracking().ToListAsync();
            return customers;
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            var customer = await _context.Customers.Include(a => a.Address).Where(c => c.Id == id && c.Active).SingleOrDefaultAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
