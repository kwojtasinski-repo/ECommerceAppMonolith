using ECommerce.Modules.Contacts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DAL.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.CompanyName).HasMaxLength(150);
            builder.Property(c => c.FirstName).HasMaxLength(50);
            builder.Property(c => c.PhoneNumber).HasMaxLength(20);
            builder.Property(c => c.LastName).HasMaxLength(50);
            builder.Property(c => c.NIP).HasMaxLength(16);
            builder.HasIndex(c => c.NIP).IsUnique();
            builder.HasIndex(c => c.PhoneNumber);
        }
    }
}
