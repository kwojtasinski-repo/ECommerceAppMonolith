using ECommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Events
{
    internal record CurrencyRateReceived(Guid CurrencyId, decimal rate) : IEvent;
}
