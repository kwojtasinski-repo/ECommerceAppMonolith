using ECommerce.Modules.Items.Domain.Events;
using ECommerce.Modules.Items.Domain.Exceptions;
using ECommerce.Shared.Abstractions.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Domain.Entities
{
    public class Type : AggregateRoot
    {
        public string Name { get; private set; }

        public ICollection<Item>? Items { get; private set; }

        public Type(AggregateId id, string name, int version = 0, ICollection<Item>? items = null)
        {
            Id = id;
            Name = name;
            Version = version;
            Items = items;
        }

        private Type(AggregateId id)
        {
            Id = id;
        }

        public static Type Create(AggregateId id, string name)
        {
            var type = new Type(id);
            type.ChangeName(name);
            type.ClearEvents();
            type.Version = 0;
            type.AddEvent(new TypeCreated(type));
            return type;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new TypeNameCannotBeEmptyException();
            }

            Name = name;
            IncrementVersion();
            AddEvent(new TypeNameChanged(this));
        }
    }
}
