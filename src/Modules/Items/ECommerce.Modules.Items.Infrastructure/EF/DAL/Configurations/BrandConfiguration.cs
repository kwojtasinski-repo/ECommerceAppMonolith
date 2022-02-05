using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.Id);
            builder
                .Property(i => i.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));
            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
            builder
                .Property(b => b.Version)
                .IsConcurrencyToken();
        }
    }
}
