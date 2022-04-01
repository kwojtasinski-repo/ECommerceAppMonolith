using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Sales.Infrastructure.EF.Configurations
{
    internal class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.CurrencyCode).IsRequired().HasMaxLength(3);
            builder.Property(i => i.RateDate).IsRequired();
            builder.Property(i => i.Created).IsRequired();
            builder.Property(i => i.Rate).IsRequired().HasPrecision(14, 4);
            builder.HasIndex(i => i.CurrencyCode);
            builder.HasIndex(i => new { i.CurrencyCode, i.Created }).IsUnique();
        }
    }
}
