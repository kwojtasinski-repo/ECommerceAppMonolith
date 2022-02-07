using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class BrandNameTooLongException : ECommerceException
    {
        public string Name { get; }

        public BrandNameTooLongException(string name) : base($"Brand Name: '{name}' is longer than 100 characters.")
        {
            Name = name;
        }
    }
}
