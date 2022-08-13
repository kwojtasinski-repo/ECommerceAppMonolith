using ECommerce.Modules.Items.Application.Files.Interfaces;

namespace ECommerce.Modules.Items.Infrastructure.Files.Implementations
{
    internal class DirectoryWrapper : IDirectoryWrapper
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
