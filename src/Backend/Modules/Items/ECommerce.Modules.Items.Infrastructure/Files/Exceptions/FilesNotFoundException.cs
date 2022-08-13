using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Infrastructure.Files.Exceptions
{
    internal class FilesNotFoundException : ECommerceException
    {
        public FilesNotFoundException() : base("Files not found.")
        {
        }
    }
}
