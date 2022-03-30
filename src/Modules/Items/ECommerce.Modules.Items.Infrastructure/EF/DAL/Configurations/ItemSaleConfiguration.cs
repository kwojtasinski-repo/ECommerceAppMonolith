using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
{
    internal class ItemSaleConfiguration : IEntityTypeConfiguration<ItemSale>
    {
        public void Configure(EntityTypeBuilder<ItemSale> builder)
        {
            builder.HasKey(i => i.Id);
            builder
                .Property(i => i.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));
            builder.Property(i => i.Cost).IsRequired().HasPrecision(14, 4);
            builder.Property(i => i.Active).HasDefaultValue(true);
            builder.Property(i => i.CurrencyCode).IsRequired().HasMaxLength(3).HasDefaultValue("PLN");
            builder.HasOne(i => i.Item).WithOne(i => i.ItemSale).HasForeignKey<ItemSale>(i => i.ItemId);
        }
    }
}
