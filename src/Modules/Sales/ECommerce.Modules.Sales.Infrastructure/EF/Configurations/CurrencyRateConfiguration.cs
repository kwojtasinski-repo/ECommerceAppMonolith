using ECommerce.Modules.Sales.Domain.Currency.Entities;
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
            builder.Property(i => i.Created).IsRequired().HasDefaultValue(DateOnly.FromDateTime(DateTime.UtcNow));
            builder.Property(i => i.Rate).IsRequired().HasPrecision(14, 4);
        }
    }
}
