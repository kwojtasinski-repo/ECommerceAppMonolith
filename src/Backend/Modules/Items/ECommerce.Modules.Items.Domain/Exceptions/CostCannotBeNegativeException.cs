using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class CostCannotBeNegativeException : ECommerceException
    {
        public decimal Cost { get; }

        public CostCannotBeNegativeException(decimal cost) : base($"Cost '{cost}' cannot be negative.")
        {
            Cost = cost;
        }
    }
}
