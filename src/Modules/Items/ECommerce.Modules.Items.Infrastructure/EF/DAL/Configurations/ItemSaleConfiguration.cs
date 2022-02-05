using ECommerce.Modules.Items.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
{
    internal class ItemSaleConfiguration : IEntityTypeConfiguration<ItemSale>
    {
        public void Configure(EntityTypeBuilder<ItemSale> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Cost).IsRequired().HasPrecision(14, 4);
            builder.Property(i => i.Active).HasDefaultValue(true);
        }
    }
}
