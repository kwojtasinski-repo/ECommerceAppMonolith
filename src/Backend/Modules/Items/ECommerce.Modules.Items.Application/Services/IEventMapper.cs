using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Services
{
    public interface IEventMapper
    {
        IMessage Map(IDomainEvent @event);
        IEnumerable<IMessage> MapAll(IEnumerable<IDomainEvent> events);
    }
}
