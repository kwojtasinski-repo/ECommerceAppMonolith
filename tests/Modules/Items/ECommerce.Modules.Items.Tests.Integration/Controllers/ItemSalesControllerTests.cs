using ECommerce.Modules.Items.Application.Commands.ItemSales;
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
    public class ItemSalesControllerTests : BaseTest, IAsyncLifetime
    {
        [Fact]
        public async Task should_return_item_sales()
        {
            var response = (await client.Request($"{Path}").GetAsync());
            var itemsFromDb = await response.GetJsonAsync<IEnumerable<ItemSaleDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemsFromDb.ShouldNotBeNull();
            itemsFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_item_sale()
        {
            var item = _items[1];
            var id = item.Id.Value;

            var response = (await client.Request($"{Path}/{id}").GetAsync());
            var itemFromDb = await response.GetJsonAsync<ItemSaleDetailsDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.Id.ShouldBe(id);
            itemFromDb.Cost.ShouldBe(item.Cost);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var item = _items[1];
            var id = item.Id.Value;
            var command = new UpdateItemSale(id, 10000M, "PLN");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").PutJsonAsync(command));
            var itemFromDb = await dbContext.ItemSales.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.Cost.ShouldBe(command.ItemCost);
            itemFromDb.Cost.ShouldNotBe(item.Cost);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var item = _items[1];
            var id = item.Id.Value;
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}/{id}").DeleteAsync());
            var itemFromDb = await dbContext.ItemSales.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_command_should_add()
        {
            var item = new Domain.Entities.Item(Guid.NewGuid(), "Item #1", new Domain.Entities.Brand(Guid.NewGuid(), "Brand #12345678"), new Domain.Entities.Type(Guid.NewGuid(), "Type #12345678"), "description #1", null,
                new Dictionary<string, IEnumerable<Domain.Entities.ValueObjects.ItemImage>> 
                {
                    { 
                        Item.IMAGES, new List<Domain.Entities.ValueObjects.ItemImage>
                        {
                           new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = true }, new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = false } 
                        }
                    }
                });
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();
            var command = new CreateItemSale(item.Id.Value, 5000M, "PLN");
            Authenticate(Guid.NewGuid(), client);

            var response = (await client.Request($"{Path}").PostJsonAsync(command));
            var id = response.GetIdFromHeaders<Guid>(Path);
            var itemSale = dbContext.ItemSales.Where(c => c.Id == id)
                .Include(i => i.Item).AsNoTracking().SingleOrDefault();

            itemSale.ShouldNotBeNull();
            itemSale.Cost.ShouldBe(command.ItemCost);
            itemSale.Item.ItemName.ShouldBe(item.ItemName);
        }

        public async Task InitializeAsync()
        {
            _items = await AddSampleData();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private async Task<List<Domain.Entities.ItemSale>> AddSampleData()
        {
            var items = GetSampleData();
            var item1 = items[0];
            var item2 = items[1];
            await dbContext.ItemSales.AddAsync(item1);
            await dbContext.ItemSales.AddAsync(item2);
            await dbContext.SaveChangesAsync();
            return items;
        }

        private List<Domain.Entities.ItemSale> GetSampleData()
        {
            var item1 = new Domain.Entities.Item(Guid.NewGuid(), "Item #1", new Domain.Entities.Brand(Guid.NewGuid(), "Brand #12345678"), new Domain.Entities.Type(Guid.NewGuid(), "Type #12345678"), "description #1", null,
                new Dictionary<string, IEnumerable<Domain.Entities.ValueObjects.ItemImage>>
                {
                    {
                        Item.IMAGES, new List<Domain.Entities.ValueObjects.ItemImage>
                        {
                           new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = true }, new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = false }
                        }
                    }
                });
            var item2 = new Domain.Entities.Item(Guid.NewGuid(), "Item #2", new Domain.Entities.Brand(Guid.NewGuid(), "Brand #123456789"), new Domain.Entities.Type(Guid.NewGuid(), "Type #123456789"), "description #2", null,
                new Dictionary<string, IEnumerable<Domain.Entities.ValueObjects.ItemImage>>
                {
                    {
                        Item.IMAGES, new List<Domain.Entities.ValueObjects.ItemImage>
                        {
                           new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = true }, new Domain.Entities.ValueObjects.ItemImage { Url = "http://localhost", MainImage = false }
                        }
                    }
                });
            var itemSale1 = new Domain.Entities.ItemSale(Guid.NewGuid(), item1, 1500M, true, "PLN");
            var itemSale2 = new Domain.Entities.ItemSale(Guid.NewGuid(), item2, 2500M, true, "PLN");
            return new List<Domain.Entities.ItemSale> { itemSale1, itemSale2 };
        }

        private const string Path = "items-module/item-sales";
        private List<ItemSale> _items = [];

        public ItemSalesControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
            : base(factory, dbContext)
        {
        }
    }
}
