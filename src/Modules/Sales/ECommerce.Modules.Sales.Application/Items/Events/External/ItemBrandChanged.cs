using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemBrandChanged(Guid Id, Guid BrandId, string BrandName) : IEvent;
}
