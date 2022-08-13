using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class BrandNameCannotBeEmptyException : ECommerceException
    {
        public BrandNameCannotBeEmptyException() : base("Brand Name cannot be empty.")
        {
        }
    }
}
