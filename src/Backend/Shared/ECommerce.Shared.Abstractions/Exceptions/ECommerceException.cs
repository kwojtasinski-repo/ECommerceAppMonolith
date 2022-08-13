﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Exceptions
{
    public class ECommerceException : Exception
    {
        protected ECommerceException(string message) : base(message)
        {
        }        
    }
}
