using ECommerce.Modules.Items.Application.Events;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Events;
using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Services
{
    internal class EventMapper : IEventMapper
    {
        public IMessage Map(IDomainEvent @event)
        {
            IMessage mappedEvent = @event switch
            {
                // Brand
                BrandCreated e => new BrandAdded(e.Brand.Id),
                Domain.Events.BrandNameChanged e => new Application.Events.BrandNameChanged(e.Brand.Id, e.Brand.Name),

                // Type
                TypeCreated e => new TypeAdded(e.Type.Id),
                Domain.Events.TypeNameChanged e => new Application.Events.TypeNameChanged(e.Type.Id, e.Type.Name),

                // Item ///TODO: sprawdzic czy te eventy beda uzyteczne jesli nie uwspolnic do jednego konkretnego
                ItemCreated e => new ItemAdded(e.Item.Id, e.Item.ItemName, e.Item.Description,
                                    e.Item.Brand.Name, e.Item.Type.Name, e.Item.Tags,
                                    e.Item.ImagesUrl?[Item.IMAGES]?.Select(i => i.Url)),
                Domain.Events.ItemBrandChanged e => new Application.Events.ItemBrandChanged(e.Item.Id, e.Item.Brand.Id, e.Item.Brand.Name),
                Domain.Events.ItemTypeChanged e => new Application.Events.ItemTypeChanged(e.Item.Id, e.Item.Type.Id, e.Item.Type.Name),
                Domain.Events.ItemNameChanged e => new Application.Events.ItemNameChanged(e.Item.Id, e.Item.ItemName),
                Domain.Events.ItemDescriptionChanged e => new Application.Events.ItemDescriptionChanged(e.Item.Id, e.Item.Description),
                Domain.Events.ItemTagsChanged e => new Application.Events.ItemTagsChanged(e.Item.Id, e.Item.Tags),
                Domain.Events.ItemImagesChanged e => new Application.Events.ItemImagesChanged(e.Item.Id, e.Item.ImagesUrl?[Item.IMAGES]?.Select(i => i.Url)),

                // ItemSale
                ItemSaleCreated e => new ItemSaleAdded(e.ItemSale.Id, e.ItemSale.ItemId, e.ItemSale.Cost, e.ItemSale.CurrencyCode),
                Domain.Events.ItemSaleCostChanged e => new Application.Events.ItemSaleCostChanged(e.ItemSale.Id, e.ItemSale.Cost),

                _ => null
            };

            return mappedEvent;
        }

        public IEnumerable<IMessage> MapAll(IEnumerable<IDomainEvent> events)
        {
            var mappedEvents = events.Select(Map);
            return mappedEvents;
        }
    }
}
