using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Application.Policies.Image;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ECommerce.Modules.Items.Application.Commands.Images.Handlers
{
    internal sealed class CreateImagesHandler : ICommandHandler<CreateImages, IEnumerable<string>>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IFileStore _fileStore;
        private readonly ISaveFilePolicy _saveFilePolicy;

        public CreateImagesHandler(IImageRepository imageRepository, IFileStore fileStore, ISaveFilePolicy saveFilePolicy)
        {
            _imageRepository = imageRepository;
            _fileStore = fileStore;
            _saveFilePolicy = saveFilePolicy;
        }

        public async Task<IEnumerable<string>> HandleAsync(CreateImages command)
        {
            if (command.Files == null || (command.Files != null && command.Files.Count == 0))
            {
                throw new ImagesCannotBeEmptyException();
            }

            ValidImages(command.Files);
            var ids = new List<string>();

            foreach (var file in command.Files)
            {
                var fileDirection = await _fileStore.WriteFileAsync(file, _saveFilePolicy.GetFileDirectory());
                var id = Guid.NewGuid();
                var image = Image.Create(id, fileDirection, file.Name);
                await _imageRepository.AddAsync(image);
                ids.Add(id.ToString());
            }

            return ids;
        }

        private void ValidImages(ICollection<IFormFile> images)
        {
            var errors = new StringBuilder();

            // FIRST VALIDATION
            foreach (var image in images)
            {
                var size = image.Length;
                var fileName = image.FileName;

                if (size > _saveFilePolicy.GetAllowedSize())
                {
                    errors.Append("Image ").Append(fileName).Append(" is too big (").Append(size).Append(" bytes). Allowed ").Append(_saveFilePolicy.GetAllowedSize()).Append("bytes\r\n");
                }

                var extension = _fileStore.GetFileExtension(fileName);
                var containsExtension = _saveFilePolicy.GetAllowedImagesExtensions().Contains(extension);

                if (!containsExtension)
                {
                    var sb = new StringBuilder();
                    _saveFilePolicy.GetAllowedImagesExtensions().ToList().ForEach(i => sb.AppendLine(i));
                    errors.AppendLine($"Image {fileName} extension {extension} is not allowed. Allowed extensions {sb}");
                }
            }

            // ERRORS OCCUERD
            if (errors.Length > 0)
            {
                throw new InvalidFilesException(errors.ToString());
            }
        }
    }
}
