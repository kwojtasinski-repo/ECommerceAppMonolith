using ECommerce.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.DAL
{
    internal class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
            // problem z UTC Date
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("users");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<User>().HasData(new User() 
            { 
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Email = "admin@admin.com",
                Role = "admin",
                Password = new PasswordHasher<User>().HashPassword(default, "PasW0Rd!26"),
                Claims = new Dictionary<string, IEnumerable<string>>
                {
                    { "permissions", new string[] { "users", "items", "item-sale", "currencies" } }
                },
                IsActive = true
            });
        }
    }
}
