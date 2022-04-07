using ECommerce.Modules.Items.Tests.Integration.Common;
using ECommerce.Modules.Sales.Application.Orders.DTO;
using ECommerce.Modules.Sales.Infrastructure.EF;
using ECommerce.Shared.Tests;
using Flurl.Http;
using Shouldly;
using System.Net;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Integration.Controllers
{
    public class CartControllerTests : BaseIntegrationTest, IClassFixture<TestApplicationFactory<Program>>,
        IClassFixture<TestSalesDbContext>
    {
        [Fact]
        public async Task should_return_my_items_in_cart()
        {
            await AddSampleData();
            Authenticate(_userId, _client);

            var response = await _client.Request($"{Path}/me").GetAsync();
            var orderItems = await response.GetJsonAsync<IEnumerable<OrderItemDto>>();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            orderItems.ShouldNotBeNull();
            orderItems.ShouldNotBeEmpty();
            orderItems.Count().ShouldBe(3);
        }

        private async Task AddSampleData()
        {
            var currentDateTime = DateTime.UtcNow;

            // Item
            var images = new string[] { "https://ithardware.pl/admin/ckeditor/filemanager/userfiles/DanielGorecki/2022/Stycze%C5%84/galaxy_s22.jpg?time=1643274614771", "https://files.mgsm.pl//news/15953/samsung-galaxy-s22-ultra-large.jpg", "https://i.wpimg.pl/O/730x0/m.komorkomania.pl/obraz-2021-09-27-130821-afe03036.png" };
            var item1 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "Description...", null, images);
            var item2 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #2", "Brand #1", "Type #1", "Description...", null, images);
            var item3 = new Domain.ItemSales.Entities.Item(Guid.NewGuid(), "Item #3", "Brand #1", "Type #1", "Description...", null, images);

            _dbContext.Items.Add(item1);
            _dbContext.Items.Add(item2);
            _dbContext.Items.Add(item3);

            // ItemSale
            var itemSale1 = new Domain.ItemSales.Entities.ItemSale(new Guid("9889049c-c94f-4f67-8f1e-ae4d1f73f456"), item1, 1000M, "PLN");
            var itemSale2 = new Domain.ItemSales.Entities.ItemSale(new Guid("7e75cbb8-9cc1-45f5-b69a-54fdf622b774"), item2, 2000M, "EUR");
            var itemSale3 = new Domain.ItemSales.Entities.ItemSale(new Guid("9120c2c5-c3fc-4283-9975-b9c8e6354bfc"), item3, 4000M, "USD");

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
            var orderItem1 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart1.Id, itemCart1, itemCart1.Price.Value, "PLN", decimal.One, _userId);
            var orderItem2 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart2.Id, itemCart2, itemCart2.Price.Value, "EUR", decimal.One, _userId);
            var orderItem3 = new Domain.Orders.Entities.OrderItem(Guid.NewGuid(), itemCart3.Id, itemCart3, itemCart3.Price.Value, "USD", decimal.One, _userId);

            _dbContext.OrderItems.Add(orderItem1);
            _dbContext.OrderItems.Add(orderItem2);
            _dbContext.OrderItems.Add(orderItem3);

            await _dbContext.SaveChangesAsync();
        }

        private Guid _userId = Guid.NewGuid();
        private const string Path = "sales-module/cart";
        private readonly IFlurlClient _client;
        private readonly SalesDbContext _dbContext;

        public CartControllerTests(TestApplicationFactory<Program> factory, TestSalesDbContext dbContext)
        {
            _client = new FlurlClient(factory.CreateClient());
            _dbContext = dbContext.DbContext;
        }
    }
}
