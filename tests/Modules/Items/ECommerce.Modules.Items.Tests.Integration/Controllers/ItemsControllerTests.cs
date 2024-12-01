﻿using ECommerce.Modules.Items.Application.Commands.Items;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    public class ItemsControllerTests : BaseTest, IAsyncLifetime
    {
        [Fact]
        public async Task should_return_items()
        {
            var response = (await client.Request($"{Path}").GetAsync());
            var itemsFromDb = await response.GetJsonAsync<IEnumerable<ItemDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemsFromDb.ShouldNotBeNull();
            itemsFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_item()
        {
            var item = _items[1];
            var id = item.Id.Value;

            var response = (await client.Request($"{Path}/{id}").GetAsync());
            var itemFromDb = await response.GetJsonAsync<ItemDetailsDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.Id.ShouldBe(id);
            itemFromDb.ItemName.ShouldBe(item.ItemName);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var item = _items[1];
            var id = item.Id.Value;
            var command = new UpdateItem(id, "Item #1234", "description 1234556675673", item.Brand.Id, item.Type.Id, new[] { "tag #1", "tag #2" }, null);
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").PutJsonAsync(command));
            var itemFromDb = await dbContext.Items.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.ItemName.ShouldBe(command.ItemName);
            itemFromDb.ItemName.ShouldNotBe(item.ItemName);
            itemFromDb.Description.ShouldBe(command.Description);
            itemFromDb.Description.ShouldNotBe(item.Description);
            itemFromDb.Tags.ShouldNotBeNull();
            itemFromDb.Tags.Count().ShouldBe(command.Tags.Count());
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var item = _items[1];
            var id = item.Id.Value;
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").DeleteAsync());
            var itemFromDb = await dbContext.Items.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_command_should_add()
        {
            var brand = new Domain.Entities.Brand(new Guid(), "Brand #1");
            var type = new Domain.Entities.Type(new Guid(), "Type #1");
            dbContext.Brands.Add(brand);
            dbContext.Types.Add(type);
            await dbContext.SaveChangesAsync();
            var tags = new[] { "tag #1", "tag #2" };
            var images = new List<ImageUrl> { new ImageUrl { Url = "http://localhost", MainImage = true }, new ImageUrl { Url = "http://localhost", MainImage = false } };
            var command = new CreateItem("Item #251234", "Description 12348012", brand.Id.Value, type.Id.Value, tags, images);
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}").PostJsonAsync(command));
            var id = response.GetIdFromHeaders<Guid>(Path);
            var item = dbContext.Items.Where(c => c.Id == id).SingleOrDefault();

            item.ShouldNotBeNull();
            item.ItemName.ShouldBe(command.ItemName);
            item.ItemName.ShouldBe(command.ItemName);
            item.Description.ShouldBe(command.Description);
            item.Tags.ShouldNotBeNull();
            item.Tags.Count().ShouldBe(command.Tags.Count());
            item.ImagesUrl[Item.IMAGES].ShouldNotBeNull();
            item.ImagesUrl[Item.IMAGES].Count().ShouldBe(command.ImagesUrl.Count());
        }

        public async Task InitializeAsync()
        {
            _items = await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task<List<Domain.Entities.Item>> AddSampleData()
        {
            var items = GetSampleData();
            var item1 = items[0];
            var item2 = items[1];
            await dbContext.Items.AddAsync(item1);
            await dbContext.Items.AddAsync(item2);
            await dbContext.SaveChangesAsync();
            return items;
        }

        private List<Domain.Entities.Item> GetSampleData()
        {
            var item1 = new Domain.Entities.Item(Guid.NewGuid(), "Item #1", new Domain.Entities.Brand(Guid.NewGuid(), "Brand #12345678") , new Domain.Entities.Type(Guid.NewGuid(), "Type #12345678") , "description #1", null,  null);
            var item2 = new Domain.Entities.Item(Guid.NewGuid(), "Item #2", new Domain.Entities.Brand(Guid.NewGuid(), "Brand #123456789"), new Domain.Entities.Type(Guid.NewGuid(), "Type #123456789"), "description #2", null,  null);
            return new List<Domain.Entities.Item> { item1, item2 };
        }

        private const string Path = "items-module/items";
        private List<Item> _items = [];

        public ItemsControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
            : base(factory, dbContext)
        {
        }
    }
}
