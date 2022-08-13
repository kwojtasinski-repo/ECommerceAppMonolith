using ECommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Events
{
    public record ItemAdded(Guid Id, string ItemName, string? Description, string BrandName, string TypeName, IEnumerable<string>? Tags, IEnumerable<string>? ImagesUrl) : IEvent;
}
