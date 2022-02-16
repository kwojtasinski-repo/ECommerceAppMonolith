using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Tests.Integration.Common
{
    public class TestItemsDbContext : IDisposable
    {
        internal ItemsDbContext DbContext { get; }

        public TestItemsDbContext()
        {
            DbContext = new ItemsDbContext(DbHelper.GetOptions<ItemsDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}
