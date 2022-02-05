using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class TypeCannotBeNullException : ECommerceException
    {
        public TypeCannotBeNullException() : base("Type cannot be null.")
        {
        }
    }
}
