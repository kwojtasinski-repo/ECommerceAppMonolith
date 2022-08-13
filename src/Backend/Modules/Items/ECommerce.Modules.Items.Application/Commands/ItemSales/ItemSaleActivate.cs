using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales
{
    public record ItemSaleActivate(Guid ItemSaleId) : ICommand;
}
