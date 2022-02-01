using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Exceptions
{
    public record ExceptionResponse(object Response, HttpStatusCode HttpStatusCode);
}
