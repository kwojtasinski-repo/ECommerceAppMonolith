using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.ItemsSale
{
    public record CreateItemSale(Guid ItemId, decimal ItemCost) : ICommand
    {
        public Guid ItemSaleId { get; } = Guid.NewGuid();
    };
}
