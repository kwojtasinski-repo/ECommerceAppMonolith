using ECommerce.Modules.Currencies.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Currencies.Core.DAL.Configurations
{
    internal static class SeedData
    {
        public static void SeedCurrency(this EntityTypeBuilder<Currency> builder)
        {
            builder.HasData(new Currency
            {
                Id = new Guid("77657829-cfb1-4603-858e-e1b66477a8e9"),
                Code = "PLN",
                Description = "Polski złoty",
            });
        }
    }
}
