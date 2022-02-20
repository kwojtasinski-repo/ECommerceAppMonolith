using ECommerce.Modules.Items.Application.Commands.Types;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Mappings
{
    public static class Extensions
    {
        public static Dictionary<string, IEnumerable<ItemImage>>? ToImageDictionary(this IEnumerable<ImageUrl> imagesUrl)
        {
            Dictionary<string, IEnumerable<ItemImage>>? urls = imagesUrl is not null ? new Dictionary<string, IEnumerable<ItemImage>>
                (new List<KeyValuePair<string, IEnumerable<ItemImage>>>() { new KeyValuePair<string, IEnumerable<ItemImage>>(Item.IMAGES,
                imagesUrl.Select(im=> new ItemImage{ Url = im.Url, MainImage = im.MainImage})) }) : null;

            return urls;
        }

        public static IEnumerable<ImageUrl>? ToImagesEnumerable(this Dictionary<string, IEnumerable<ItemImage>> images)
        {
            if (!images.Any())
            {
                return Enumerable.Empty<ImageUrl>();
            }

            images.TryGetValue(Item.IMAGES, out var enumerableImages);
            return enumerableImages?.Select(im => im.AsImageUrl());
        }

        public static ImageUrl AsImageUrl(this ItemImage itemImage)
        {
            var imageUrl = new ImageUrl
            {
                Url = itemImage.Url,
                MainImage = itemImage.MainImage
            };
            return imageUrl;
        }

        public static BrandDto AsDto(this Brand brand)
        {
            var brandDto = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
            return brandDto;
        }

        public static ItemDetailsDto AsDetailsDto(this Item item)
        {
            var itemDto = new ItemDetailsDto
            {
                Id = item.Id,
                Brand = item.Brand.AsDto(),
                Description = item.Description,
                ItemName = item.ItemName,
                Tags = item.Tags,
                Type = item.Type.AsDto(),
                ImagesUrl = item.ImagesUrl?.ToImagesEnumerable()
            };
            return itemDto;
        }

        public static ItemDto AsDto(this Item item)
        {
            var itemDto = new ItemDto
            {
                Id = item.Id,
                Brand = item.Brand.AsDto(),
                ItemName = item.ItemName,
                Type = item.Type.AsDto(),
                ImagesUrl = item.ImagesUrl?.ToImagesEnumerable().Where(i => i.MainImage == true).SingleOrDefault()
            };
            return itemDto;
        }

        public static TypeDto AsDto(this Domain.Entities.Type type)
        {
            var typeDto = new TypeDto
            {
                Id = type.Id,
                Name = type.Name
            };
            return typeDto;
        }

        public static ItemSaleDto AsDto(this ItemSale itemSale)
        {
            var itemSaleDto = new ItemSaleDto
            {
                Id = itemSale.Id,
                Cost = itemSale.Cost,
                Active = itemSale.Active.Value != null && itemSale.Active.Value is true,
                Item = new ItemToSaleDto(itemSale.Item.Id, itemSale.Item.ItemName,
                        itemSale.Item.ImagesUrl.ToImagesEnumerable().Where(i => i.MainImage == true).SingleOrDefault())
            };
            return itemSaleDto;
        }

        public static ItemSaleDetailsDto AsDetailsDto(this ItemSale itemSale)
        {
            var itemSaleDto = new ItemSaleDetailsDto
            {
                Id = itemSale.Id,
                Cost = itemSale.Cost,
                Active = itemSale.Active.Value != null && itemSale.Active.Value is true,
                Item = itemSale.Item.AsDetailsDto()
            };
            return itemSaleDto;
        }
    }
}
