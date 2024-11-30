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
                Id = Guid.Parse("e70b6db8-f77a-4ce7-833f-977617cf1873"),
                CreatedAt = new DateTime(2022, 8, 13, 16, 59, 53, 177, DateTimeKind.Local).AddTicks(589),
                Email = "admin@admin.com",
                Role = "admin",
                // PasW0Rd!26
                Password = "AQAAAAEAACcQAAAAEP/+MBJ+0Y0ditII5cclQrsBB8G7mJyZ+y3zBn0yfFoHiSF/RiZCWSdemZ+eQ70Vag==",
                Claims = new Dictionary<string, IEnumerable<string>>
                {
                    { "permissions", new string[] { "users", "items", "item-sale", "currencies" } }
                },
                IsActive = true
            });
        }
    }
}
