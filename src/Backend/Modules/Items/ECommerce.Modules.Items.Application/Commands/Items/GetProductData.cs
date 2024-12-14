using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items
{
    public record GetProductData(Guid ProductId) : ICommand<ProductDataDto?>;
}
