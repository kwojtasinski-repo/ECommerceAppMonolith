using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Tests.Integration.Common
{
    public class TestSalesDbContext : IDisposable
    {
        internal SalesDbContext DbContext { get; }

        public TestSalesDbContext()
        {
            DbContext = new SalesDbContext(DbHelper.GetOptions<SalesDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}
