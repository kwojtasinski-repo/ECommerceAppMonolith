using ECommerce.Modules.Sales.Application.Payments.DTO;

namespace ECommerce.Modules.Sales.Application.Orders.DTO
{
    public class OrderDetailsDto : OrderDto
    {
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
        public IEnumerable<PaymentDto>? Payments { get; set; }
    }
}
