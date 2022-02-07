using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TypeCannotBeDeletedException : ECommerceException
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public TypeCannotBeDeletedException(Guid id, string name) : base($"Type '{name}' with id: '{id}' cannot be deleted.")
        {
            Id = id;
            Name = name;
        }
    }
}
