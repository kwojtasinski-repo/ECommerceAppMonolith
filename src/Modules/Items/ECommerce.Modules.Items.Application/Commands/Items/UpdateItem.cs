using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.Items
{
    public record UpdateItem(Guid ItemId, string ItemName, string? Description, Guid BrandId, Guid TypeId, IEnumerable<string>? Tags, IEnumerable<ImageUrl>? ImagesUrl) : ICommand;
}
