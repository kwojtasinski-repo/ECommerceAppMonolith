using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TypeNameTooLongException : ECommerceException
    {
        public string Name { get; }

        public TypeNameTooLongException(string name) : base($"Type Name: '{name}' is longer than 100 characters.")
        {
            Name = name;
        }
    }
}
