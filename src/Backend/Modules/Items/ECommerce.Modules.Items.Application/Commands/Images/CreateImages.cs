using ECommerce.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Modules.Items.Application.Commands.Images
{
    public record CreateImages(IList<IFormFile> Files) : ICommand<IEnumerable<string>>;
}
