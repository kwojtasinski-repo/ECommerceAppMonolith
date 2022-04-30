using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Configurations
{
    internal class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        private readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ItemName).IsRequired().HasMaxLength(150);
            builder.Property(i => i.BrandName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.TypeName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Description).HasMaxLength(300);

            // save Tags as text
            builder
                .Property(i => i.Tags)
                .HasConversion(tags => string.Join(',', tags), tags => tags.Split(',', StringSplitOptions.RemoveEmptyEntries));

            // determine how to compare Tags
            builder
                .Property(x => x.Tags).Metadata.SetValueComparer(
                new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));


            // save as json collection and deserialize ImagesUrl
            builder.Property(u => u.ImagesUrl)
                .HasConversion(u => JsonSerializer.Serialize(u, SerializerOptions),
                    u => JsonSerializer.Deserialize<IEnumerable<string>>(u, SerializerOptions));

            // determine how to compare ImagesUrl
            builder.Property(u => u.ImagesUrl).Metadata.SetValueComparer(
                new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));
        }
    }
}
