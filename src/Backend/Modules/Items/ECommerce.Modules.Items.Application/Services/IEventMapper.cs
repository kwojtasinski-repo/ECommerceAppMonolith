using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;

namespace ECommerce.Modules.Items.Application.Services
{
    public interface IEventMapper
    {
        IMessage? Map(IDomainEvent @event);
        IEnumerable<IMessage> MapAll(IEnumerable<IDomainEvent> events);
    }
}
