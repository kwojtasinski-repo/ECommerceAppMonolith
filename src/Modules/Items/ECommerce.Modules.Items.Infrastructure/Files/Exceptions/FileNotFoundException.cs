using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Infrastructure.Files.Exceptions
{
    internal class FileNotFoundException : ECommerceException
    {
        public FileNotFoundException() : base("File not found")
        {
        }
    }
}
