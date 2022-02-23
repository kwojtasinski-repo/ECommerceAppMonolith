using ECommerce.Modules.Sales.Domain.Orders.Entities;
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
        public Order Order { get; private set; }

        private Payment() { }

        public Payment(AggregateId id, string paymentNumber, Order order)
        {
            ValidatePayment(paymentNumber);
            Id = id;
            PaymentNumber = paymentNumber;
            Order = order;
        }

        public static Payment Create(string paymentNumber, Order order)
        {
            var payment = new Payment(Guid.NewGuid(), paymentNumber, order);
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
