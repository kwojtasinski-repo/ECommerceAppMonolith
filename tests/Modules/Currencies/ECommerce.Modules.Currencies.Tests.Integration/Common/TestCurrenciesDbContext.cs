using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Shared.Tests;
using System;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    public class TestCurrenciesDbContext : IDisposable
    {
        internal CurrenciesDbContext DbContext { get; }

        public TestCurrenciesDbContext()
        {
            DbContext = new CurrenciesDbContext(DbHelper.GetOptions<CurrenciesDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}