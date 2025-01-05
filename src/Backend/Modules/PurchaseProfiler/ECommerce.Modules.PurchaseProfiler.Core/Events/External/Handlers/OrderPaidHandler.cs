using ECommerce.Modules.PurchaseProfiler.Core.Clients;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal class OrderPaidHandler : IEventHandler<OrderPaid>
    {
        private readonly ILogger<OrderPaidHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserApiClient _userApiClient;
        private readonly IProductApiClient _productApiClient;

        public OrderPaidHandler(ILogger<OrderPaidHandler> logger, IOrderRepository orderRepository,
            IUserRepository userRepository, IProductRepository productRepository,
            IUserApiClient userApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _userApiClient = userApiClient;
            _productApiClient = productApiClient;
        }

        public async Task HandleAsync(OrderPaid @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                var user = await GetUser(@event.UserId);
                if (user is null)
                {
                    _logger.LogError("User with id '{userId}' was not found", @event.UserId);
                    return;
                }

                var boughtItemsId = @event.OrderItems.Select(oi => oi.ItemId).Distinct().ToList();
                var boughtItems = await GetProducts(boughtItemsId);

                await _orderRepository.AddAsync(new Entities.Order
                {
                    OrderId = @event.Id,
                    TotalCost = @event.Price,
                    OrderDate = @event.PaymentDate,
                    CustomerId = @event.CustomerId,
                    UserId = @event.UserId,
                    Items = @event.OrderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        ItemKey = boughtItemsId.Contains(oi.ItemId) ? boughtItems[oi.ItemId]?.KeyValue ?? 0 : 0,
                        Cost = oi.Price
                    }).ToList() ?? []
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(OrderPaidHandler));
            }
        }

        private async Task<User?> GetUser(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is not null)
            {
                return user;
            }

            var userFromApi = await _userApiClient.GetUser(userId);
            if (userFromApi is null)
            {
                return null;
            }

            user = new User { UserId = userFromApi.UserId, Email = userFromApi.Email };
            await _userRepository.AddAsync(user);
            return user;
        }

        private async Task<Dictionary<Guid, Product>> GetProducts(List<Guid> productsId)
        {
            var products = (await _productRepository.GetProductsByItemsIdsAsync(productsId))
                    .ToDictionary(key => key.ProductId) ?? [];

            foreach (var productId in productsId)
            {
                if (products.ContainsKey(productId))
                {
                    continue;
                }

                var product = await GetProduct(productId);
                if (product is null)
                {
                    _logger.LogError("Product with id '{productId}' was not found", productId);
                    continue;
                }

                products.Add(productId, product);
            }

            return products;
        }

        private async Task<Product?> GetProduct(Guid productId)
        {
            var productFromApi = await _productApiClient.GetProduct(productId);
            if (productFromApi is null)
            {
                return null;
            }

            return await _productRepository.AddAsync(new Product
            {
                ProductId = productFromApi.ProductId,
                ProductSaleId = productFromApi.ProductSaleId,
                Cost = productFromApi.Cost,
                IsActivated = productFromApi.IsActivated,
            });
        }
    }
}
