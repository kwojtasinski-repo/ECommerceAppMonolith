namespace ECommerce.Modules.Sales.Application.Orders.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreateOrderDate { get; set; }
        public DateTime? OrderApprovedDate { get; set; }
        public decimal Cost { get; set; }
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public bool Paid { get; set; }
    }
}
