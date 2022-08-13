using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales
{
    public record DeleteItemSale(Guid ItemSaleId) : ICommand
    {
    };
}
