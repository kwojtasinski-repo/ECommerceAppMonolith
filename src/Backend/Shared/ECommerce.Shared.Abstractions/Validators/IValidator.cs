using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Validators
{
    public interface IValidator<T> where T : class
    {
        void Validate(T validator);
    }
}
