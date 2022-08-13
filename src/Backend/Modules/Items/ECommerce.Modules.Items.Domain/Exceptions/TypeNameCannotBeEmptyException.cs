﻿using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class TypeNameCannotBeEmptyException : ECommerceException
    {
        public TypeNameCannotBeEmptyException() : base("Type Name cannot be empty.")
        {
        }
    }
}
