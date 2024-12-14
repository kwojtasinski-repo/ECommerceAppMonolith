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
        private readonly IUserCustomerMapRepository _userCustomerMapRepository;
        private readonly IUserApiClient _userApiClient;

        public OrderPaidHandler(ILogger<OrderPaidHandler> logger, IOrderRepository orderRepository,
            IUserRepository userRepository, IProductRepository productRepository, IUserCustomerMapRepository userCustomerMapRepository,
            IUserApiClient userApiClient)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _userCustomerMapRepository = userCustomerMapRepository;
            _userApiClient = userApiClient;
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

                var userCustomersMap = await _userCustomerMapRepository.GetAllByUserIdAsync(@event.UserId);
                var userCustomerMap = userCustomersMap.FirstOrDefault(u => u.CustomerId == @event.CustomerId);
                userCustomerMap ??= await _userCustomerMapRepository.AddAsync(new UserCustomersMap { CustomerId = @event.CustomerId, UserId = @event.UserId });

                var boughtItemsId = @event.OrderItems.Select(oi => oi.ItemId).Distinct().ToList();
                var boughtItems = await _productRepository.GetProductsByItemsIdsAsync(boughtItemsId);

                await _orderRepository.AddAsync(new Entities.Order
                {
                    OrderId = @event.Id,
                    TotalCost = @event.Price,
                    OrderDate = @event.PaymentDate,
                    CustomerId = @event.CustomerId,
                    CustomerKey = userCustomerMap.KeyValue,
                    UserId = @event.UserId,
                    Items = @event.OrderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        ItemKey = boughtItems.FirstOrDefault(i => oi.ItemId == i.ProductId)?.KeyValue ?? 0,
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
    }
}
