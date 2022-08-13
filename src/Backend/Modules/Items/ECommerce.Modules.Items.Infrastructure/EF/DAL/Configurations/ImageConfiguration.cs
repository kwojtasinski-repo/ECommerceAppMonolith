using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);
            builder
                .Property(i => i.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));
            builder.Property(i => i.ImageName).IsRequired().HasMaxLength(100);

            builder
                .Property(i => i.Version)
                .IsConcurrencyToken();
        }
    }
}
