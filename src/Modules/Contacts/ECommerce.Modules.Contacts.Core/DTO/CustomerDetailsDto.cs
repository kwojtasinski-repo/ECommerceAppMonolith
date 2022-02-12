using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DTO
{
    internal class CustomerDetailsDto : CustomerDto
    {
        public AddressDto Address { get; set; }
    }
}
