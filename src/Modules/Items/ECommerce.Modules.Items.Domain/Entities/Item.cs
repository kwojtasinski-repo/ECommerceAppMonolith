using ECommerce.Modules.Items.Domain.Entities.ValueObjects;
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
        public Type Type { get; private set; }
        public ItemSale? ItemSale { get; private set; }

        public IEnumerable<string>? Tags { get; private set; }
        public Dictionary<string, IEnumerable<ItemImage>>? ImagesUrl { get; private set; }

        public Item(AggregateId id, string itemName, Brand brand, Type type, string? description, IEnumerable<string> tags, Dictionary<string, IEnumerable<ItemImage>> imagesUrl, int version = 0)
        {
            Validate(brand, type);
            Id = id;
            ItemName = itemName;
            Brand = brand;
            Type = type;
            Description = description;
            Tags = tags;
            ImagesUrl = imagesUrl;
            Version = version;
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

        public static Item Create(AggregateId id, string itemName, Brand brand, Type type, string description, IEnumerable<string>? tags = null, Dictionary<string, IEnumerable<ItemImage>>? imagesUrl = null)
        {
            var item = new Item(id);
            item.ChangeName(itemName);
            item.ChangeBrand(brand);
            item.ChangeType(type);
            item.ChangeDescription(description);
            item.ChangeTags(tags);
            item.ChangeImagesUrl(imagesUrl);

            item.ClearEvents();
            item.Version = 0;

            return item;
        }

        public void ChangeName(string name)
        {
            if (ItemName == name)
            {
                return;
            }

            ItemName = name;
            IncrementVersion();
        }

        public void ChangeBrand(Brand brand)
        {
            if (brand is null)
            {
                throw new BrandCannotBeNullException();
            }

            if (Brand.Id == brand.Id)
            {
                return;
            }

            Brand = brand;
            IncrementVersion();
        }

        public void ChangeType(Type type)
        {
            if (type is null)
            {
                throw new TypeCannotBeNullException();
            }

            if (Type.Id == type.Id)
            {
                return;
            }

            Type = type;
            IncrementVersion();
        }

        public void ChangeDescription(string? description)
        {
            if (Description == description)
            {
                return;
            }

            Description = description;
            IncrementVersion();
        }

        public void ChangeTags(IEnumerable<string>? tags)
        {
            Tags = tags;
            IncrementVersion();
        }

        public void ChangeImagesUrl(Dictionary<string, IEnumerable<ItemImage>>? imagesUrl)
        {
            ImagesUrl = imagesUrl;
            IncrementVersion();
        }
    }
}
