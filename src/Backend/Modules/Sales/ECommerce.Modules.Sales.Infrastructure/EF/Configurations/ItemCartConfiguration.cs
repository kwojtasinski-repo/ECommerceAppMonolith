using ECommerce.Modules.Sales.Domain.Orders.Entities;
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
    internal class ItemCartConfiguration : IEntityTypeConfiguration<ItemCart>
    {
        private readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public void Configure(EntityTypeBuilder<ItemCart> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.ItemName).IsRequired().HasMaxLength(150);
            builder.Property(i => i.BrandName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.TypeName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Description).HasMaxLength(300);
            builder.OwnsOne(i => i.Price, navigation =>
            {
                navigation.Property(m => m.Value).HasColumnName("Cost").IsRequired().HasPrecision(14, 4);
            });
            builder.Property(i => i.CurrencyCode).IsRequired().HasMaxLength(3);
            builder.HasIndex(i => i.CurrencyCode);

            // save Tags as text
            builder
                .Property(i => i.Tags)
                .HasConversion(tags => string.Join(',', tags), tags => tags.Split(',', StringSplitOptions.None));

            // determine how to compare Tags
            builder
                .Property(x => x.Tags).Metadata.SetValueComparer(
                new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));


            // save as json collection and deserialize to ImagesUrl
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
