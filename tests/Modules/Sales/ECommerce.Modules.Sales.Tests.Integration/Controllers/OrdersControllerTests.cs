using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using ECommerce.Modules.Sales.Application.Orders.Commands;
using Flurl.Http;
using Xunit;
using System.Net;
using Shouldly;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Modules.Sales.Tests.Integration.Controllers
{
    [Collection("integrationSales")]
    public class OrdersControllerTests : IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestSalesDbContext>
    {
        [Fact]
        public async Task given_valid_order_items_with_different_currencies_should_create_order_with_valid_cost_and_currency()
        {
            await AddSampleData();
            var expectedCurrency = "USD";
            var request = new CreateOrder(Guid.NewGuid(), expectedCurrency);
            var expectedCost = 8500; // 4000USD, 1000PLN, 2000EUR
            Authenticate(_userId);

            var response = await _client.Request($"{Path}").PostJsonAsync(request);
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();
            responseHeaderValue.ShouldNotBeNull();
            var splitted = responseHeaderValue.Split(Path + '/');
            Guid.TryParse(splitted[1], out var id);
            var order = _dbContext.Orders.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            order.Cost.ShouldBe(expectedCost);
            order.CurrencyCode.ShouldBe(expectedCurrency);
        }

        private async Task AddSampleData()
        {
            // Currency
            _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = DateOnly.FromDateTime(DateTime.Now), CurrencyCode = "PLN", Rate = 1, RateDate = DateOnly.FromDateTime(DateTime.Now) });
            _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = DateOnly.FromDateTime(DateTime.Now), CurrencyCode = "USD", Rate = 2, RateDate = DateOnly.FromDateTime(DateTime.Now) });
            _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = DateOnly.FromDateTime(DateTime.Now), CurrencyCode = "EUR", Rate = 4, RateDate = DateOnly.FromDateTime(DateTime.Now) });

            // Item
            var item1 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description...", null, null);
            var item2 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #2", "Brand #1", "Type #1", "Description...", null, null);
            var item3 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #3", "Brand #1", "Type #1", "Description...", null, null);

            _dbContext.Items.Add(item1);
            _dbContext.Items.Add(item2);
            _dbContext.Items.Add(item3);

            // ItemSale
            var itemSale1 = new Domain.ItemSales.Entities.ItemSale(Guid.NewGuid(), item1, 1000M, "PLN");
            var itemSale2 = new Domain.ItemSales.Entities.ItemSale(Guid.NewGuid(), item2, 2000M, "EUR");
            var itemSale3 = new Domain.ItemSales.Entities.ItemSale(Guid.NewGuid(), item3, 4000M, "USD");

            _dbContext.ItemSales.Add(itemSale1);
            _dbContext.ItemSales.Add(itemSale2);
            _dbContext.ItemSales.Add(itemSale3);

            // ItemCart
            var images = new string [] { "https://ithardware.pl/admin/ckeditor/filemanager/userfiles/DanielGorecki/2022/Stycze%C5%84/galaxy_s22.jpg?time=1643274614771", "https://files.mgsm.pl//news/15953/samsung-galaxy-s22-ultra-large.jpg", "https://i.wpimg.pl/O/730x0/m.komorkomania.pl/obraz-2021-09-27-130821-afe03036.png" };
            var itemCart1 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description...", null, images, 1000M, "PLN");
            var itemCart2 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #2", "Brand #1", "Type #1", "Description...", null, images, 2000M, "EUR");
            var itemCart3 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #3", "Brand #1", "Type #1", "Description...", null, images, 4000M, "USD");

            _dbContext.ItemCarts.Add(itemCart1);
            _dbContext.ItemCarts.Add(itemCart2);
            _dbContext.ItemCarts.Add(itemCart3);

            // OrderItem
            var orderItem1 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart1.Id, itemCart1, itemCart1.Cost, "PLN", decimal.One, _userId);
            var orderItem2 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart2.Id, itemCart2, itemCart2.Cost, "EUR", decimal.One, _userId);
            var orderItem3 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart3.Id, itemCart3, itemCart3.Cost, "USD", decimal.One, _userId);

            _dbContext.OrderItems.Add(orderItem1);
            _dbContext.OrderItems.Add(orderItem2);
            _dbContext.OrderItems.Add(orderItem3);

            await _dbContext.SaveChangesAsync();
        }

        private void Authenticate(Guid userId)
        {
            var claims = new Dictionary<string, IEnumerable<string>>();
            claims.Add("permissions", new[] { "items", "item-sale" });
            var jwt = AuthHelper.GenerateJwt(userId.ToString(), "admin", claims: claims);
            _client.WithOAuthBearerToken(jwt);
        }

        private Guid _userId = Guid.NewGuid();
        private const string Path = "sales-module/orders";
        private readonly IFlurlClient _client;
        private readonly SalesDbContext _dbContext;

        public OrdersControllerTests(TestApplicationFactory<Program> factory, TestSalesDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}