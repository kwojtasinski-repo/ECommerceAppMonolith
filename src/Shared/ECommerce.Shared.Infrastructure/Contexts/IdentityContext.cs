using ECommerce.Shared.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Contexts
{
    internal class IdentityContext : IIdentityContext
    {
        public bool IsAuthenticated { get; }
        public Guid Id { get; }
        public string Role { get; }
        public Dictionary<string, IEnumerable<string>> Claims { get; }

        public IdentityContext(ClaimsPrincipal principal)
        {
            IsAuthenticated = principal.Identity?.IsAuthenticated is true;
            Id = IsAuthenticated ? Guid.Parse(principal.Identity.Name) : Guid.Empty;
            Role = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            Claims = principal.Claims.GroupBy(c => c.Type)
                .ToDictionary(c => c.Key, x => x.Select(c => c.Value.ToString()));
        }
    }
}
