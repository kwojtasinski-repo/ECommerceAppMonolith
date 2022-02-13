using ECommerce.Modules.Contacts.Core.Entities;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
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
        private readonly IClock _clock;
        private readonly IContext _context;

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
            // problem z UTC Date
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options, IClock clock, IContext context) : base(options)
        {
            // problem z UTC Date
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _clock = clock;
            _context = context;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("contacts");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _context.Identity.Id;
                        entry.Entity.Created = _clock.CurrentDate();
                        entry.Entity.Active = true;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _context.Identity.Id;
                        entry.Entity.Modified = _clock.CurrentDate();
                        break;
                    case EntityState.Deleted:
                        entry.Entity.ModifiedBy = _context.Identity.Id;
                        entry.Entity.Modified = _clock.CurrentDate();
                        entry.Entity.InactivedBy = _context.Identity.Id;
                        entry.Entity.Inactived = _clock.CurrentDate();
                        entry.Entity.Active = false;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
