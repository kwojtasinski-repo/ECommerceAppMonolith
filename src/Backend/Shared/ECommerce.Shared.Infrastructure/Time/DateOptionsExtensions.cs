using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Time
{
    internal static class DateOptionsExtensions
    {
        public static MvcOptions UseDateOnlyTimeOnlyStringConverters(this MvcOptions options)
        {
            TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
            return options;
        }
    }
}
