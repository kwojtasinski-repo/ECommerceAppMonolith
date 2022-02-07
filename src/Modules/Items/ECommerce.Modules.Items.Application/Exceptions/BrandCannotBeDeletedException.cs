using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class BrandCannotBeDeletedException : ECommerceException
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public BrandCannotBeDeletedException(Guid id, string name) : base($"Brand '{name}' with id: '{id}' cannot be deleted.")
        {
            Id = id;
            Name = name;
        }
    }
}
