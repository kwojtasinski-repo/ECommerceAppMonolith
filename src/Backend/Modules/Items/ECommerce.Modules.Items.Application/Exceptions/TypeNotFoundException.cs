using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TypeNotFoundException : ECommerceException
    {
        public Guid Id { get; }

        public TypeNotFoundException(Guid id) : base($"Type with id: '{id}' was not found.")
        {
            Id = id;
        }
    }
}
