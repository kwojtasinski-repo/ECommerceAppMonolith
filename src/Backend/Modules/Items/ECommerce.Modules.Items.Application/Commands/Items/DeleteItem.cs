using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items
{
    public record DeleteItem(Guid ItemId) : ICommand;
}
