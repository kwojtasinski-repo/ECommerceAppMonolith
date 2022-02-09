using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Application.Queries.Images;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Images.Handlers
{
    internal sealed class GetImageHandler : IQueryHandler<GetImage, ImageDto>
    {
        private readonly DbSet<Image> _images;
        private readonly IFileStore _fileStore;

        public GetImageHandler(ItemsDbContext itemsDbContext, IFileStore fileStore)
        {
            _images = itemsDbContext.Images;
            _fileStore = fileStore;
        }

        public async Task<ImageDto> HandleAsync(GetImage query)
        {
            var image = await _images.Where(i => i.Id == query.ImageId).AsNoTracking().SingleOrDefaultAsync();

            if (image is null)
            {
                return null;
            }

            var bytes = await _fileStore.ReadFileAsync(image.SourcePath);
            var base64String = Convert.ToBase64String(bytes);
            var extension = _fileStore.GetFileExtenstion(image.SourcePath);

            return new ImageDto { ImageSource = base64String, Extension = extension };
        }
    }
}
