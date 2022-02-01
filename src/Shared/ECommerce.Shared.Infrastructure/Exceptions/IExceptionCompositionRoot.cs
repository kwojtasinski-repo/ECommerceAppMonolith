using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Exceptions
{
    internal interface IExceptionCompositionRoot
    {
        ExceptionResponse Map(Exception exception);
    }
}
