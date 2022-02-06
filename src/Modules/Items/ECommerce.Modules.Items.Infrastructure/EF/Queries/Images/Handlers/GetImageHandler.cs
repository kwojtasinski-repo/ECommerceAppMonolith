using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Application.Queries.Images;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Infrastructure.EF.Queries.Images.Handlers
{
    internal sealed class GetImageHandler : IQueryHandler<GetImage, ImageDto>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IFileStore _fileStore;

        public GetImageHandler(IImageRepository imageRepository, IFileStore fileStore)
        {
            _imageRepository = imageRepository;
            _fileStore = fileStore;
        }

        public async Task<ImageDto> HandleAsync(GetImage query)
        {
            var image = await _imageRepository.GetAsync(query.ImageId);

            if (image is null)
            {
                return null;
            }

            var bytes = await _fileStore.ReadFileAsync(image.SourcePath);
            var base64String = Convert.ToBase64String(bytes);

            return new ImageDto { ImageSource = base64String };
        }
    }
}
