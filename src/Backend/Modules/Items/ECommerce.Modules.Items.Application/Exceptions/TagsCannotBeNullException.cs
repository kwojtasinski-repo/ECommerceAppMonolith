using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TagsCannotBeNullException : ECommerceException
    {
        public TagsCannotBeNullException() : base("Tags cannot be null.")
        {
        }
    }
}
