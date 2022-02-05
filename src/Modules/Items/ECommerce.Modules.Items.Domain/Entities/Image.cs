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
        public bool? MainImage { get; private set; }
        public Guid? ItemId { get; private set; }

        public Image(AggregateId id, string sourcePath, string imageName, bool mainImage = false, Guid? itemId = null)
        {
            Id = id;
            SourcePath = sourcePath;
            ImageName = imageName;
            MainImage = mainImage;
            ItemId = itemId;
        }

        private Image(AggregateId id, string sourcePath, Guid? itemId = null)
        {
            Id = id;
            SourcePath = sourcePath;
            ItemId = itemId;
        }

        internal static Image Create(Guid id, string sourcePath, string imageName, bool mainImage = false, Guid? itemId = null)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new SourcePathCannotBeNullException();
            }

            var image = new Image(id, sourcePath, itemId);
            image.ChangeName(imageName);

            if (mainImage) 
            {
                image.SetMainImage();
            }

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

        public void SetMainImage()
        {
            MainImage = true;
            IncrementVersion();
        }
    }
}
