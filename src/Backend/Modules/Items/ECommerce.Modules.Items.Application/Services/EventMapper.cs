using ECommerce.Modules.Items.Application.Events;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Events;
using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;

namespace ECommerce.Modules.Items.Application.Services
{
    internal class EventMapper : IEventMapper
    {
        public IMessage? Map(IDomainEvent @event)
        {
            IMessage? mappedEvent = @event switch
            {
                // Brand
                BrandCreated e => new BrandAdded(e.Brand.Id),
                Domain.Events.BrandNameChanged e => new Application.Events.BrandNameChanged(e.Brand.Id, e.Brand.Name),

                // Type
                TypeCreated e => new TypeAdded(e.Type.Id),
                Domain.Events.TypeNameChanged e => new Application.Events.TypeNameChanged(e.Type.Id, e.Type.Name),

                // Item
                ItemCreated e => new ItemAdded(e.Item.Id, e.Item.ItemName, e.Item.Description,
                                    e.Item.Brand.Name, e.Item.Type.Name, e.Item.Tags,
                                    e.Item.ImagesUrl?[Item.IMAGES]?.Select(i => i.Url)),

                // ItemSale
                ItemSaleCreated e => new ItemSaleAdded(e.ItemSale.Id, e.ItemSale.ItemId, e.ItemSale.Cost, e.ItemSale.CurrencyCode),
                ItemSaleActivate e => new ItemSaleActivated(e.ItemSale.Id),
                ItemSaleDeactivate e => new ItemSaleDeactivated(e.ItemSale.Id),

                _ => null
            };

            return mappedEvent;
        }

        public IEnumerable<IMessage> MapAll(IEnumerable<IDomainEvent> events)
        {
            IEnumerable<IMessage> mappedEvents = events.Select(Map).OfType<IMessage>() ?? [];
            return mappedEvents;
        }
    }
}
