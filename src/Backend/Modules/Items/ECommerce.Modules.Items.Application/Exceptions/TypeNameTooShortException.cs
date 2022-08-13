using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TypeNameTooShortException : ECommerceException
    {
        public string Name { get; }

        public TypeNameTooShortException(string name) : base($"Type Name: '{name}' is shorter than 3 characters.")
        {
            Name = name;
        }
    }
}
