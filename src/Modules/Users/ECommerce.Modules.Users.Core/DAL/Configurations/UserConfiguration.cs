using ECommerce.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            // zapis do tabeli jako json i deserializacja jako kolekcja
            builder.Property(u => u.Claims)
                .HasConversion(c => JsonSerializer.Serialize(c, SerializerOptions), 
                 c => JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(c, SerializerOptions));

            // okreslenie jak maja byc porownywane wartosci w polu Claims
            builder.Property(u => u.Claims).Metadata.SetValueComparer(
                new ValueComparer<Dictionary<string, IEnumerable<string>>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary(x => x.Key, x => x.Value)));
        }
    }
}
