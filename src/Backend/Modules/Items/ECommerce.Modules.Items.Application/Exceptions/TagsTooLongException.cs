using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Exceptions
{
    internal class TagsTooLongException : ECommerceException
    {
        public IEnumerable<string> Tags { get; }

        public TagsTooLongException(IEnumerable<string> tags) : base($"Tags '{string.Join(',', tags)}' are too long.")
        {
            Tags = tags;
        }
    }
}
