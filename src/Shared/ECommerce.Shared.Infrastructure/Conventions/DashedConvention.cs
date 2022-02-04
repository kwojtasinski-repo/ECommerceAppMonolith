using Humanizer;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Conventions
{
    internal class DashedConvention : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) { return null; }

            var routeName = value.ToString().Kebaberize();

            return routeName;
        }
    }
}
