using ECommerce.Modules.Items.Domain.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    public class Item : AggregateRoot
    {
        public string ItemName { get; private set; }
        public string? Description { get; private set; }
        public Brand Brand { get; private set; }
        public Guid BrandId { get; private set; }
        public Type Type { get; private set; }
        public Guid TypeId { get; private set; }

        public IEnumerable<string>? Tags { get; private set; }
        public Dictionary<string, IEnumerable<string>>? ImagesUrl { get; private set; }

        public ICollection<Image>? Images { get; private set; }

        public Item(AggregateId id, string itemName, Brand brand, Type type, string? description = null, IEnumerable<string>? tags = null, Dictionary<string, IEnumerable<string>>? imagesUrl = null)
        {
            Validate(brand, type);
            Id = id;
            ItemName = itemName;
            Brand = brand;
            BrandId = brand.Id;
            Type = type;
            TypeId = type.Id;
            Description = description;
            Tags = tags;
            ImagesUrl = imagesUrl;
        }

        private static void Validate(Brand brand, Type type)
        {
            if (brand is null)
            {
                throw new BrandCannotBeNullException();
            }
            if (type is null)
            {
                throw new TypeCannotBeNullException();
            }
        }

        private Item(AggregateId id)
        {
            Id = id;
        }

        public static Item Create(AggregateId id, string itemName, Brand brand, Type type, string description, IEnumerable<string>? tags = null)
        {
            var item = new Item(id);
            item.ChangeName(itemName);
            item.ChangeBrand(brand);
            item.ChangeType(type);
            item.ChangeDescription(description);
            item.Tags = tags;

            item.ClearEvents();
            item.Version = 0;

            return item;
        }

        public void ChangeName(string name)
        {
            ItemName = name;
            IncrementVersion();
        }

        public void ChangeBrand(Brand brand)
        {
            if (brand is null)
            {
                throw new BrandCannotBeNullException();
            }

            Brand = brand;
            BrandId = brand.Id;
            IncrementVersion();
        }

        public void ChangeType(Type type)
        {
            if (type is null)
            {
                throw new TypeCannotBeNullException();
            }

            Type = type;
            TypeId = type.Id;
            IncrementVersion();
        }

        public void ChangeDescription(string description)
        {
            Description = description;
            IncrementVersion();
        }
    }
}
