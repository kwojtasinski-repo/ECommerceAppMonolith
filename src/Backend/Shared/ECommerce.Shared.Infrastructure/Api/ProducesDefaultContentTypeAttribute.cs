using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Api
{
    public class ProducesDefaultContentTypeAttribute : ProducesAttribute
    {
        public ProducesDefaultContentTypeAttribute(Type type) : base(type)
        {
        }

        public ProducesDefaultContentTypeAttribute(params string[] additionalContentTypes)
            : base("application/json", additionalContentTypes)
        {
        }
    }
}
