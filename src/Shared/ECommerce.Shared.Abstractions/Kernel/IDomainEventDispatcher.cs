using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Kernel
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(params IDomainEvent[] events);
    }
}
