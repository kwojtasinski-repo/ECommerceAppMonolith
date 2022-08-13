using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Shared.Abstractions.Kernel;

namespace ECommerce.Modules.Items.Domain.Events
{
    public record ItemSaleActivate(ItemSale ItemSale) : IDomainEvent;
}
