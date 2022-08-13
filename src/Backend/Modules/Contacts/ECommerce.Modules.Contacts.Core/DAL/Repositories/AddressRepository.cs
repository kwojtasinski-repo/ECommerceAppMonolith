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
    internal class AddressRepository : IAddressRepository
    {
        private readonly ContactsDbContext _context;

        public AddressRepository(ContactsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetAsync(Guid id)
        {
            var address = await _context.Addresses.Where(a => a.Id == id && a.Active).SingleOrDefaultAsync();
            return address;
        }

        public async Task UpdateAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
