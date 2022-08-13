using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Validators
{
    public sealed class ValidationException : ECommerceException
    {
        public IEnumerable<ECommerceException> Exceptions { get; }

        public ValidationException(IEnumerable<ECommerceException> exceptions) : base($"{GetErrorMessages(exceptions)}")
        {
            Exceptions = exceptions;
        }

        private static string GetErrorMessages(IEnumerable<ECommerceException> exceptions)
        {
            var message = 
                string.Join("\n", exceptions.Select(e => e.Message));
            return message;
        }
    }
}