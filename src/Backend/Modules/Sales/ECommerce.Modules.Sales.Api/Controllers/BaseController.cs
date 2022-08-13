using ECommerce.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Api.Controllers
{
    [ApiController]
    [Route(SalesModule.BasePath + "/[controller]")]
    [ProducesDefaultContentType]
    internal class BaseController : ControllerBase
    {
        protected ActionResult<T> OkOrNotFound<T>(T model)
        {
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        protected void AddResourceIdHeader(Guid id) => Response.Headers.Add("Resource-ID", id.ToString());
    }
}
