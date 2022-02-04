using ECommerce.Shared.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Contexts
{
    internal interface IContextFactory
    {
        IContext Create();
    }
}
