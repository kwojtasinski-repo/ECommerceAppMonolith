using ECommerce.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Currencies.Api.Controllers
{
    [ApiController]
    [Route(CurrenciesModule.BasePath + "/[controller]")]
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
    }
}
