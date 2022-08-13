﻿using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    public class OrderItemsCannotBeNullException : ECommerceException
    {
        public OrderItemsCannotBeNullException() : base("OrderItems cannot be null.")
        {
        }
    }
}
