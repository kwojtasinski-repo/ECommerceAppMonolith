using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class BrandNameTooShortException : ECommerceException
    {
        public string Name { get; }

        public BrandNameTooShortException(string name) : base($"Brand Name: '{name}' is shorter than 3 characters.")
        {
            Name = name;
        }
    }
}
