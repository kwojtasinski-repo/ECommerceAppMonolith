namespace ECommerce.Modules.Sales.Application.Payments.DTO
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid UserId { get; set; }
    }
}
