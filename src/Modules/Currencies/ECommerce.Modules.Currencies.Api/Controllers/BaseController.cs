﻿using ECommerce.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Api.Controllers
{
    [ApiController]
    [Route(CurrenciesModule.BasePath + "/[controller]")]
    [ProducesDefaultContentType]
    internal class BaseController : ControllerBase
    {
    }
}
