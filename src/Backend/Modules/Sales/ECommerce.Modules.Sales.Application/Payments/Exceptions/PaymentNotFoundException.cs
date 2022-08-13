using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Payments.Exceptions
{
    internal class PaymentNotFoundException : ECommerceException
    {
        public Guid PaymentId { get; }

        public PaymentNotFoundException(Guid paymentId) : base($"Payment with id '{paymentId}' was not found.")
        {
            PaymentId = paymentId;
        }
    }
}
