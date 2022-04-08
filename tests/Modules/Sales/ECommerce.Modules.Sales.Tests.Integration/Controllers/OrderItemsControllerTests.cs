using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Integration.Controllers
{
    public class OrderItemsControllerTests : BaseIntegrationTest, IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestSalesDbContext>
    {
        [Fact]
        public async Task given_valid_command_should_add_order_item()
        {
            await AddSampleData();
            var itemSale = _dbContext.ItemSales.FirstOrDefault();
            var command = new CreateOrderItem(itemSale.Id, "EUR");
            Authenticate(_userId, _client);

            var response = await _client.Request($"{Path}").PostJsonAsync(command);
            var id = response.GetIdFromHeaders<Guid>(Path);

            var orderItem = _dbContext.OrderItems.Where(oi => oi.Id == id).AsNoTracking().SingleOrDefault();
            orderItem.ShouldNotBeNull();
            orderItem.Currency.CurrencyCode.ShouldBe(command.CurrencyCode);
        }

        [Fact]
        public async Task given_valid_id_should_delete_order_item()
        {
            await AddSampleData();
            var itemSale = _dbContext.ItemSales.FirstOrDefault();
            var command = new CreateOrderItem(itemSale.Id, "EUR");
            Authenticate(_userId, _client);
            var responseAdded = await _client.Request($"{Path}").PostJsonAsync(command);
            var id = responseAdded.GetIdFromHeaders<Guid>(Path);

            var response = await _client.Request($"{Path}/{id}").DeleteAsync();

            response.StatusCode.ShouldBe((int) HttpStatusCode.OK);
            var orderItem = _dbContext.OrderItems.Where(oi => oi.Id == id).AsNoTracking().SingleOrDefault();
            orderItem.ShouldBeNull();
        }

        private async Task AddSampleData()
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

            await _dbContext.SaveChangesAsync();
        }


        private Guid _userId = Guid.NewGuid();
        private const string Path = "sales-module/order-items";
        private readonly IFlurlClient _client;
        private readonly SalesDbContext _dbContext;

        public OrderItemsControllerTests(TestApplicationFactory<Program> factory, TestSalesDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
