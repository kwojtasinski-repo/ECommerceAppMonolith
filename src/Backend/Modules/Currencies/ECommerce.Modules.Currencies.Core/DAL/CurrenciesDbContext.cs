﻿using ECommerce.Modules.Currencies.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Currencies.Core.DAL
{
    internal class CurrenciesDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options) : base(options)
        {
            // UTC Date problem
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("currencies");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
