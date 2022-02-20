using ECommerce.Modules.Items.Application.Commands.ItemSales;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Infrastructure.EF.DAL;
using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Integration.Controllers
{
    [Collection("integrationItemSales")]
    public class ItemSalesControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestItemsDbContext>
    {
        [Fact]
        public async Task should_return_item_sales()
        {
            await AddSampleData();

            var response = (await _client.Request($"{Path}").GetAsync());
            var itemsFromDb = await response.GetJsonAsync<IEnumerable<ItemSaleDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemsFromDb.ShouldNotBeNull();
            itemsFromDb.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_valid_id_should_return_item_sale()
        {
            var items = await AddSampleData();
            var item = items[1];
            var id = item.Id.Value;

            var response = (await _client.Request($"{Path}/{id}").GetAsync());
            var itemFromDb = await response.GetJsonAsync<ItemSaleDetailsDto>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.Id.ShouldBe(id);
            itemFromDb.Cost.ShouldBe(item.Cost);
        }

        [Fact]
        public async Task given_valid_command_should_update()
        {
            var items = await AddSampleData();
            var item = items[1];
            var id = item.Id.Value;
            var command = new UpdateItemSale(id, 10000M);
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PutJsonAsync(command));
            var itemFromDb = await _dbContext.ItemSales.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            itemFromDb.ShouldNotBeNull();
            itemFromDb.Cost.ShouldBe(command.ItemCost);
            itemFromDb.Cost.ShouldNotBe(item.Cost);
        }

        [Fact]
        public async Task given_valid_id_should_delete()
        {
            var items = await AddSampleData();
            var item = items[1];
            var id = item.Id.Value;
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}/{id}").DeleteAsync());
            var itemFromDb = await _dbContext.ItemSales.Where(b => b.Id == id).AsNoTracking().SingleOrDefaultAsync();

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
            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();
            var command = new CreateItemSale(item.Id.Value, 5000M);
            Authenticate(Guid.NewGuid());

            var response = (await _client.Request($"{Path}").PostJsonAsync(command));
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var itemSale = _dbContext.ItemSales.Where(c => c.Id == id)
                .Include(i => i.Item).AsNoTracking().SingleOrDefault();

            itemSale.ShouldNotBeNull();
            itemSale.Cost.ShouldBe(command.ItemCost);
            itemSale.Item.ItemName.ShouldBe(item.ItemName);
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "item-sale" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private async Task<List<Domain.Entities.ItemSale>> AddSampleData()
        {
            var items = GetSampleData();
            var item1 = items[0];
            var item2 = items[1];
            await _dbContext.ItemSales.AddAsync(item1);
            await _dbContext.ItemSales.AddAsync(item2);
            await _dbContext.SaveChangesAsync();
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
            var itemSale1 = new Domain.Entities.ItemSale(Guid.NewGuid(), item1, 1500M, true);
            var itemSale2 = new Domain.Entities.ItemSale(Guid.NewGuid(), item2, 2500M, true);
            return new List<Domain.Entities.ItemSale> { itemSale1, itemSale2 };
        }

        private const string Path = "items-module/item-sales";
        private readonly IFlurlClient _client;
        private readonly ItemsDbContext _dbContext;

        public ItemSalesControllerTests(TestApplicationFactory<Program> factory, TestItemsDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
