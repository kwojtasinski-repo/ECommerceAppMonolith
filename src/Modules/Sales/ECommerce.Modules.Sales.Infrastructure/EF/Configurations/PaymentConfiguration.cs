using ECommerce.Modules.Sales.Domain.Payments.Entities;
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
    internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));

            builder.Property(p => p.PaymentNumber).IsRequired().HasMaxLength(50);

            builder
                .Property(p => p.Version)
                .IsConcurrencyToken();

            builder.HasIndex(p => p.PaymentNumber).IsUnique();
        }
    }
}
