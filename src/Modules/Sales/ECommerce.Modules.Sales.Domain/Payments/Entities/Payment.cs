using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Payments.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;

namespace ECommerce.Modules.Sales.Domain.Payments.Entities
{
    public class Payment : AggregateRoot
    {
        public string PaymentNumber { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public Order Order { get; private set; }
        public Guid UserId { get; private set; }

        private Payment() { }

        public Payment(AggregateId id, string paymentNumber, Order order, Guid userId, DateTime paymentDate)
        {
            ValidatePayment(paymentNumber);
            Id = id;
            PaymentNumber = paymentNumber;
            Order = order;
            UserId = userId;
            PaymentDate = paymentDate;
        }

        public static Payment Create(string paymentNumber, Order order, Guid userId, DateTime paymentDate)
        {
            var payment = new Payment(Guid.NewGuid(), paymentNumber, order, userId, paymentDate);
            return payment;
        }

        private void ValidatePayment(string paymentNumber)
        {
            if (string.IsNullOrWhiteSpace(paymentNumber))
            {
                throw new PaymentNumberCannotBeEmptyException();
            }
        }
    }
}
