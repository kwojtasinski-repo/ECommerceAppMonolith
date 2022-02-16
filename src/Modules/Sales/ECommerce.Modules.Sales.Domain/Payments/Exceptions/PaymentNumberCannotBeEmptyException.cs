using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Payments.Exceptions
{
    internal class PaymentNumberCannotBeEmptyException : ECommerceException
    {
        public PaymentNumberCannotBeEmptyException() : base("PaymentNumber cannot be empty.")
        {
        }
    }
}
