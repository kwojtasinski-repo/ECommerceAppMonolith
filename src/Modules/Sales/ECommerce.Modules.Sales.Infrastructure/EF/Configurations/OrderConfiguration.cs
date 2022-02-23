using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));

            builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);

            builder
                .Property(o => o.Version)
                .IsConcurrencyToken();

            builder.Property(o => o.Cost).IsRequired().HasPrecision(14, 4);

            builder.HasIndex(o => o.OrderNumber).IsUnique();
        }
    }
}
