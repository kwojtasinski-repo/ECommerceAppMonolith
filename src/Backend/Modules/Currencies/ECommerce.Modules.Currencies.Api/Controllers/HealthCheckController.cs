using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Api.Controllers
{
    [Route(CurrenciesModule.BasePath)]
    internal class HealthCheckController : BaseController
    {
        [HttpGet]
        public ActionResult<string> Get() => "Currencies API";
    }
}
