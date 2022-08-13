using ECommerce.Modules.Currencies.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.DAL.Configurations
{
    internal class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            builder.Property(cr => cr.CurrencyId).IsRequired();
            builder.Property(cr => cr.Rate).IsRequired().HasPrecision(14,4);
            builder.Property(cr => cr.CurrencyDate).IsRequired();
        }
    }
}
