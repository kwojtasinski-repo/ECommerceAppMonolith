﻿using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Addresses
{
    internal class CityNameCannotBeEmptyException : ECommerceException
    {
        public CityNameCannotBeEmptyException() : base("CityName cannot be empty.")
        {
        }
    }
}
