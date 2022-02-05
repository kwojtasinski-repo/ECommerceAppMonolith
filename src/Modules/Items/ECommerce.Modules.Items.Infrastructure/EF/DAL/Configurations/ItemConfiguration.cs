using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
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

            builder
                .Property(i => i.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));

            builder.Property(i => i.ItemName).IsRequired().HasMaxLength(100);
            builder.Property(i => i.Description).HasMaxLength(300);

            // zapis tagow jako text
            builder
                .Property(i => i.Tags)
                .HasConversion(tags => string.Join(',', tags), tags => tags.Split(',', StringSplitOptions.None));

            builder
                .Property(i => i.Version)
                .IsConcurrencyToken();

            // okreslenie jak maja byc porownywane wartosci w polu Tags
            builder
                .Property(x => x.Tags).Metadata.SetValueComparer(
                new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode()))));

            // zapis do tabeli jako json i deserializacja jako kolekcja
            builder.Property(u => u.ImagesUrl)
                .HasConversion(c => JsonSerializer.Serialize(c, SerializerOptions),
                 c => JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(c, SerializerOptions));

            // okreslenie jak maja byc porownywane wartosci w polu ImagesUrl
            builder.Property(u => u.ImagesUrl).Metadata.SetValueComparer(
                new ValueComparer<Dictionary<string, IEnumerable<string>>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary(x => x.Key, x => x.Value)));
        }
    }
}
