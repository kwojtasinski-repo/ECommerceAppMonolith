using ECommerce.Modules.Contacts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DAL
{
    internal class ContactsDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
            // problem z UTC Date
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("contacts");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
