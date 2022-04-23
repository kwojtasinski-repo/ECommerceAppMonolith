using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using ECommerce.Modules.Sales.Application.Orders.Commands;
using Flurl.Http;
using Xunit;
using System.Net;
using Shouldly;
using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Application.Orders.DTO;

namespace ECommerce.Modules.Sales.Tests.Integration.Controllers
{
    [Collection("integrationSales")]
    public class OrdersControllerTests : BaseIntegrationTest, IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestSalesDbContext>
    {
        [Fact]
        public async Task given_valid_order_item_should_add_to_order()
        {
            var itemSales = await AddSampleData();
            var order = Order.Create(Guid.NewGuid(), "ORD/123/123", "EUR", Guid.NewGuid(), _userId, DateTime.UtcNow);
            _dbContext.Add(order);
            await _dbContext.SaveChangesAsync();
            var itemSaleId = itemSales.Where(i => i.CurrencyCode == "USD").FirstOrDefault().Id; // 4000 USD // rate 2 USD 4 EUR
            var request = new AddOrderItemToOrder(order.Id, itemSaleId);
            Authenticate(_userId, _client);

            var response = await _client.Request($"{Path}/positions/add").PatchJsonAsync(request);

            var orderFromDb = _dbContext.Orders.Include(oi => oi.OrderItems).Where(o => o.Id == order.Id).AsNoTracking().SingleOrDefault();
            orderFromDb.ShouldNotBeNull();
            orderFromDb.OrderItems.Count().ShouldBe(1);
            orderFromDb.Price.Value.ShouldBe(2000M);
        }

        [Fact]
        public async Task given_valid_order_items_with_different_currencies_should_create_order_with_valid_cost_and_currency()
        {
            await AddSampleData();
            var expectedCurrency = "USD";
            var request = new CreateOrder(Guid.NewGuid(), expectedCurrency);
            var expectedCost = 8500; // 4000USD, 1000PLN, 2000EUR rate USD 2PLN, EUR 4PLN
            Authenticate(_userId, _client);

            var response = await _client.Request($"{Path}").PostJsonAsync(request);
            var id = response.GetIdFromHeaders<Guid>(Path);
            var order = _dbContext.Orders.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            order.Price.Value.ShouldBe(expectedCost);
            order.Currency.CurrencyCode.ShouldBe(expectedCurrency);
        }

        [Fact]
        public async Task given_valid_order_item_should_delete_from_order()
        {
            await AddSampleData();
            var expectedCurrency = "USD";
            var request = new CreateOrder(Guid.NewGuid(), expectedCurrency);
            Authenticate(_userId, _client);
            var response = await _client.Request($"{Path}").PostJsonAsync(request);
            var id = response.GetIdFromHeaders<Guid>(Path);
            var order = _dbContext.Orders.Include(oi => oi.OrderItems).Where(c => c.Id == id).AsNoTracking().SingleOrDefault();
            var orderItemToDetele = order.OrderItems.FirstOrDefault();
            var requestDelete = new DeleteOrderItemFromOrder(order.Id, orderItemToDetele.Id);
            var expectedCost = 8500;

            var responseDeletedPosition = await _client.Request($"{Path}/positions/delete").PatchJsonAsync(requestDelete);

            var orderFromDb = _dbContext.Orders.Include(oi => oi.OrderItems).Where(o => o.Id == order.Id).AsNoTracking().SingleOrDefault();
            orderFromDb.ShouldNotBeNull();
            orderFromDb.OrderItems.Count().ShouldBe(2);
            orderFromDb.Price.Value.ShouldBeLessThan(expectedCost);
        }

        [Fact]
        public async Task given_valid_order_should_delete()
        {
            await AddSampleData();
            var expectedCurrency = "USD";
            var request = new CreateOrder(Guid.NewGuid(), expectedCurrency);
            Authenticate(_userId, _client);
            var response = await _client.Request($"{Path}").PostJsonAsync(request);
            var id = response.GetIdFromHeaders<Guid>(Path);

            var responseDeletedPosition = await _client.Request($"{Path}/{id}").DeleteAsync();

            var order = _dbContext.Orders.Where(c => c.Id == id).AsNoTracking().SingleOrDefault();
            responseDeletedPosition.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            order.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_customer_id_should_update_order()
        {
            var order = await AddSampleOrder();
            var customerId = Guid.NewGuid();
            var request = new ChangeCustomerInOrder(order.Id, customerId);
            Authenticate(_userId, _client);

            var response = await _client.Request($"{Path}/customer/change").PatchJsonAsync(request);

            var orderAfterUpdate = await _dbContext.Orders.Where(o => o.Id == order.Id).AsNoTracking().SingleOrDefaultAsync();
            orderAfterUpdate.ShouldNotBeNull();
            orderAfterUpdate.CustomerId.ShouldNotBe(order.CustomerId);
            orderAfterUpdate.CustomerId.ShouldBe(customerId);
        }

        [Fact]
        public async Task given_valid_order_and_currency_should_update_order()
        {
            await AddSampleData();
            var primaryCurrency = "PLN";
            var currency = "USD";
            var request = new CreateOrder(Guid.NewGuid(), primaryCurrency);
            Authenticate(_userId, _client);
            var response = await _client.Request($"{Path}").PostJsonAsync(request);
            var id = response.GetIdFromHeaders<Guid>(Path);
            var order = await _client.Request($"{Path}/{id}").GetJsonAsync<OrderDetailsDto>();
            var command = new ChangeCurrencyInOrder(id, currency);

            var responseCurrencyChanged = await _client.Request($"{Path}/currency/change").PatchJsonAsync(command);

            responseCurrencyChanged.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var orderUpdated = await _dbContext.Orders.Where(o => o.Id == id).AsNoTracking().SingleOrDefaultAsync();
            order.Cost.ShouldBeGreaterThan(orderUpdated.Price.Value);
            orderUpdated.Currency.CurrencyCode.ShouldBe(currency);
        }

        private async Task<Order> AddSampleOrder()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid().ToString("N"), Guid.NewGuid(), _userId, DateTime.UtcNow);
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        private async Task<List<Domain.ItemSales.Entities.ItemSale>> AddSampleData()
        {
            var currentDateTime = DateTime.UtcNow;
            var currentDate = DateOnly.FromDateTime(currentDateTime);

            var ratesExists = _dbContext.CurrencyRates.Any(cr => cr.Created == currentDate);

            if (!ratesExists)
            {
                // Currency
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate, CurrencyCode = "PLN", Rate = 1, RateDate = currentDate });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate, CurrencyCode = "USD", Rate = 2, RateDate = currentDate });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate, CurrencyCode = "EUR", Rate = 4, RateDate = currentDate });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-1), CurrencyCode = "PLN", Rate = 1, RateDate = currentDate.AddDays(-1) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-1), CurrencyCode = "USD", Rate = 2, RateDate = currentDate.AddDays(-1) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-1), CurrencyCode = "EUR", Rate = 4, RateDate = currentDate.AddDays(-1) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-2), CurrencyCode = "PLN", Rate = 1, RateDate = currentDate.AddDays(-2) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-2), CurrencyCode = "USD", Rate = 2, RateDate = currentDate.AddDays(-2) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-2), CurrencyCode = "EUR", Rate = 4, RateDate = currentDate.AddDays(-2) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-3), CurrencyCode = "PLN", Rate = 1, RateDate = currentDate.AddDays(-3) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-3), CurrencyCode = "USD", Rate = 2, RateDate = currentDate.AddDays(-3) });
                _dbContext.CurrencyRates.Add(new Domain.Currencies.Entities.CurrencyRate { Id = Guid.NewGuid(), Created = currentDate.AddDays(-3), CurrencyCode = "EUR", Rate = 4, RateDate = currentDate.AddDays(-3) });
            }

            // Item
            var images = new string[] { "https://ithardware.pl/admin/ckeditor/filemanager/userfiles/DanielGorecki/2022/Stycze%C5%84/galaxy_s22.jpg?time=1643274614771", "https://files.mgsm.pl//news/15953/samsung-galaxy-s22-ultra-large.jpg", "https://i.wpimg.pl/O/730x0/m.komorkomania.pl/obraz-2021-09-27-130821-afe03036.png" };
            var item1 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description...", null, images);
            var item2 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #2", "Brand #1", "Type #1", "Description...", null, images);
            var item3 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #3", "Brand #1", "Type #1", "Description...", null, images);

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
            var itemCart1 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description...", null, images, 1000M, "PLN", currentDateTime);
            var itemCart2 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #2", "Brand #1", "Type #1", "Description...", null, images, 2000M, "EUR", currentDateTime);
            var itemCart3 = new Domain.Orders.Entities.ItemCart(Guid.NewGuid(), "Item #3", "Brand #1", "Type #1", "Description...", null, images, 4000M, "USD", currentDateTime);

            _dbContext.ItemCarts.Add(itemCart1);
            _dbContext.ItemCarts.Add(itemCart2);
            _dbContext.ItemCarts.Add(itemCart3);

            // OrderItem
            var date = DateTime.UtcNow;
            var orderItem1 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart1.Id, itemCart1, itemCart1.Price.Value, "PLN", decimal.One, _userId);
            var orderItem2 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart2.Id, itemCart2, itemCart2.Price.Value, "EUR", decimal.One, _userId);
            var orderItem3 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart3.Id, itemCart3, itemCart3.Price.Value, "USD", decimal.One, _userId);

            _dbContext.OrderItems.Add(orderItem1);
            _dbContext.OrderItems.Add(orderItem2);
            _dbContext.OrderItems.Add(orderItem3);

            await _dbContext.SaveChangesAsync();
            return new List<Domain.ItemSales.Entities.ItemSale> { itemSale1, itemSale2, itemSale3 };
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