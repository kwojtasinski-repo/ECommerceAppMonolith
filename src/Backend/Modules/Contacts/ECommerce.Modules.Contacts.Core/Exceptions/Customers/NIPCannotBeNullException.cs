﻿using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class NIPCannotBeNullException : ECommerceException
    {
        public NIPCannotBeNullException() : base("NIP cannot be null.")
        {
        }
    }
}
