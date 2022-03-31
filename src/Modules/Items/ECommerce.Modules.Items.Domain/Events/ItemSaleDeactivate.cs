using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel;

namespace ECommerce.Modules.Items.Domain.Events
{
    public record ItemSaleDeactivate(ItemSale ItemSale) : IDomainEvent;
}
