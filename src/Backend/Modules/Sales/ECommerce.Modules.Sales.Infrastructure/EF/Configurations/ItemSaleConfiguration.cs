using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Configurations
{
    internal class ItemSaleConfiguration : IEntityTypeConfiguration<ItemSale>
    {
        public void Configure(EntityTypeBuilder<ItemSale> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Cost).IsRequired().HasPrecision(14, 4);
            builder.HasOne(i => i.Item).WithOne(i => i.ItemSale).HasForeignKey<ItemSale>(i => i.ItemId);
            builder.Property(i => i.CurrencyCode).IsRequired().HasMaxLength(3);
            builder.HasIndex(i => i.CurrencyCode);
        }
    }
}
