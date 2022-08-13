﻿using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Infrastructure.EF
{
    public sealed class SalesDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemSale> ItemSales { get; set; }
        public DbSet<ItemCart> ItemCarts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
            //  UTC Date problem
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("sales");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
