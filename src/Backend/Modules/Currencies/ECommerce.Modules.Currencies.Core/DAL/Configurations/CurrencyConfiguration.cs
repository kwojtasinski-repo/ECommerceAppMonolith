using ECommerce.Modules.Currencies.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Currencies.Core.DAL.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(c => c.Code).IsRequired().HasMaxLength(3);
            builder.HasIndex(c => c.Code).IsUnique();
            builder.Property(c => c.Description).IsRequired();

            builder.SeedCurrency();
        }
    }
}
