using ECommerce.Modules.Users.Core.DAL;
using ECommerce.Shared.Tests;
using System;
using Xunit;

namespace ECommerce.Modules.Users.Tests.Integration
{
    public class TestUsersDbContext : IDisposable
    {
        internal UsersDbContext DbContext { get; }

        public TestUsersDbContext()
        {
            DbContext = new UsersDbContext(DbHelper.GetOptions<UsersDbContext>());
        }

        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }
    }
}