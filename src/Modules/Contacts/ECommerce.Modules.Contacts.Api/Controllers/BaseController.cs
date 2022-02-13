using ECommerce.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Api.Controllers
{
    [ApiController]
    [ProducesDefaultContentType]
    [Route(ContactsModule.BasePath + "/[controller]")]
    internal class BaseController
    {
    }
}
