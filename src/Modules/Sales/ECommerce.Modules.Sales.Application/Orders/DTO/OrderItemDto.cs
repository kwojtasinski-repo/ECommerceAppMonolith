namespace ECommerce.Modules.Sales.Application.Orders.DTO
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ItemCartId { get; set; }
        public ItemCartDto ItemCart { get; set; }
        public Guid UserId { get; set; }
    }
}
