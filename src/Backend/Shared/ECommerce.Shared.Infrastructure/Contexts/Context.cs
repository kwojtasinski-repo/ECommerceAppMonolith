using ECommerce.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Contexts
{
    internal sealed class Context : IContext
    {
        public string RequestId { get; } = $"{Guid.NewGuid():N}";
        public string TraceId { get; }
        public IIdentityContext Identity { get; }

        internal Context()
        {
        }

        public Context(HttpContext httpContext) : this(httpContext.TraceIdentifier, new IdentityContext(httpContext.User))
        {
        }

        internal Context(string traceId, IIdentityContext identity)
        {
            TraceId = traceId;
            Identity = identity;
        }

        public static Context Empty => new Context();
    }
}
