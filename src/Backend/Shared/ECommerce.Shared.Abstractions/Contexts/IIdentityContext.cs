using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Contexts
{
    public interface IIdentityContext
    {
        bool IsAuthenticated { get; }
        Guid Id { get; }
        string Role { get; }
        Dictionary<string, IEnumerable<string>> Claims { get; }
    }
}
