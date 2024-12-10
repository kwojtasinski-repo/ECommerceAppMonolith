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

        public OrderPaidHandler(ILogger<OrderPaidHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(OrderPaid @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                await _orderRepository.AddAsync(new Entities.Order
                {
                    OrderId = @event.Id,
                    TotalCost = @event.Price,
                    OrderDate = @event.PaymentDate,
                    UserId = @event.UserId,
                    Items = @event.OrderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        Cost = oi.Price
                    }).ToList() ?? []
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(OrderPaidHandler));
            }
        }
    }
}
