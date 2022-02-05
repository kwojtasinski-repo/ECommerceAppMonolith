using ECommerce.Modules.Items.Domain.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    public class Brand : AggregateRoot
    {
        public string Name { get; private set; }
        public ICollection<Item>? Items { get; private set; }

        public Brand(AggregateId id, string name)
        {
            Id = id;
            Name = name;
        }

        private Brand(AggregateId id)
        {
            Id = id;
        }

        public static Brand Create(AggregateId id, string name)
        {
            var brand = new Brand(id);
            brand.ChangeName(name);
            brand.ClearEvents();
            brand.Version = 0;
            return brand;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BrandNameCannotBeEmptyException();
            }

            Name = name;
            IncrementVersion();
        }
    }
}
