
using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class BrandNotFoundException : ECommerceException
    {
        public Guid Id { get; }

        public BrandNotFoundException(Guid id) : base($"Brand with id: '{id}' was not found.")
        {
            Id = id;
        }
    }
}
