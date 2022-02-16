using ECommerce.Modules.Sales.Domain.Payments.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Payments.Entities
{
    public class Payment : AggregateRoot
    {
        public string PaymentNumber { get; private set; }
        public Guid OrderId { get; private set; }

        public Payment(AggregateId id, string paymentNumber, Guid orderId)
        {
            ValidatePayment(paymentNumber);
            Id = id;
            PaymentNumber = paymentNumber;
            OrderId = orderId;
        }

        public static Payment Create(string paymentNumber, Guid orderId)
        {
            var payment = new Payment(Guid.NewGuid(), paymentNumber, orderId);
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
