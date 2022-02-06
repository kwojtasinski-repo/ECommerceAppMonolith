using ECommerce.Modules.Items.Domain.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    public class Image : AggregateRoot
    {
        public string SourcePath { get; private set; }
        public string ImageName { get; private set; }

        public Image(AggregateId id, string sourcePath, string imageName, int version = 0)
        {
            Id = id;
            SourcePath = sourcePath;
            ImageName = imageName;
            Version = version;
        }

        private Image(AggregateId id, string sourcePath)
        {
            Id = id;
            SourcePath = sourcePath;
        }

        public static Image Create(Guid id, string sourcePath, string imageName)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new SourcePathCannotBeNullException();
            }

            var image = new Image(id, sourcePath);
            image.ChangeName(imageName);

            image.ClearEvents();
            image.Version = 0;

            return image;
        }

        public void ChangeName(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                throw new ImageNameCannotBeNullException();
            }

            ImageName = imageName;
            IncrementVersion();
        }
    }
}
