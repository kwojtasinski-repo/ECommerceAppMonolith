using ECommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Events
{
    public record ItemSaleAdded(Guid Id, Guid ItemId, decimal Cost, string CurrencyCode) : IEvent;
}
