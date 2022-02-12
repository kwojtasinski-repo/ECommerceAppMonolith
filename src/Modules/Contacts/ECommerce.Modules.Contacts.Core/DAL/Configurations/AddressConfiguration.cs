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
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(a => a.Customer).WithOne(c => c.Address).HasForeignKey<Address>(a => a.CustomerId);
            builder.Property(a => a.ZipCode).HasMaxLength(16);
            builder.Property(a => a.BuildingNumber).HasMaxLength(16);
            builder.Property(a => a.LocaleNumber).HasMaxLength(16);
            builder.Property(a => a.StreetName).HasMaxLength(100);
            builder.Property(a => a.CountryName).HasMaxLength(100);
            builder.Property(a => a.CityName).HasMaxLength(100);
            builder.HasIndex(a => a.ZipCode);
            builder.HasIndex(a => a.BuildingNumber);
            builder.HasIndex(a => a.LocaleNumber);
        }
    }
}
