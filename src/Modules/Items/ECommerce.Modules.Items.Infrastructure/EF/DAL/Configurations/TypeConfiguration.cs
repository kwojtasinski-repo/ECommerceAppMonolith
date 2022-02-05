using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Modules.Items.Infrastructure.EF.DAL.Configurations
{
    internal class TypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Type>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Type> builder)
        {
            builder.HasKey(t => t.Id);
            builder
                .Property(i => i.Id)
                .HasConversion(id => id.Value, id => new AggregateId(id));
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder
                .Property(t => t.Version)
                .IsConcurrencyToken();
        }
    }
}
