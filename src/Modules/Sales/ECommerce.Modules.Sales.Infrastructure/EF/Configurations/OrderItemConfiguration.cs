using ECommerce.Modules.Sales.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.HasOne(oi => oi.ItemCart).WithOne(ic => ic.OrderItem).HasForeignKey<OrderItem>(oi => oi.ItemCartId);


            builder.OwnsOne(i => i.Price, navigation =>
            {
                navigation.Property(m => m.Value).HasColumnName("Cost").IsRequired().HasPrecision(14, 4);
            });

            builder.OwnsOne(i => i.Currency, navigation =>
            {
                navigation.Property(c => c.CurrencyCode).HasColumnName("CurrencyCode").IsRequired().HasMaxLength(3);
                navigation.HasIndex(c => c.CurrencyCode);
                navigation.Property(c => c.Rate).HasColumnName("Rate").IsRequired().HasPrecision(14, 4);
            });
        }
    }
}
