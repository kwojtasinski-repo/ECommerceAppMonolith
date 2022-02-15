using ECommerce.Modules.Contacts.Core.DAL;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
using ECommerce.Shared.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Tests.Integration.Common
{
    public class TestContactsDbContext : IDisposable
    {
        internal ContactsDbContext DbContext { get; }
        private readonly IClock _clock;
        private readonly Context _context; 
        public Guid UserId { get; }

        public TestContactsDbContext()
        {
            _clock = new Clock();
            _context = new Context();
            DbContext = new ContactsDbContext(DbHelper.GetOptions<ContactsDbContext>(), _clock, _context);
            UserId = _context.Identity.Id;
        }

        public void Dispose()
        {
            DbContext?.Database.EnsureDeleted();
            DbContext?.Dispose();
        }

        private class Clock : IClock
        {
            public DateTime CurrentDate()
            {
                return DateTime.UtcNow;
            }
        }

        private class Context : IContext
        {
            public string RequestId => Guid.NewGuid().ToString("N");

            public string TraceId => Guid.NewGuid().ToString("N");

            public IIdentityContext Identity => new IdentityContext();
        }

        private class IdentityContext : IIdentityContext
        {
            public bool IsAuthenticated => true;

            public Guid Id => Guid.NewGuid();

            public string Role => "db";

            public Dictionary<string, IEnumerable<string>> Claims => DefaultDictionary();

            private Dictionary<string, IEnumerable<string>> DefaultDictionary() 
            {
                var dictionary = new Dictionary<string, IEnumerable<string>>();
                dictionary.Add("claims", new[] { "contacts" });
                return dictionary;
            }
        }
    }
}
