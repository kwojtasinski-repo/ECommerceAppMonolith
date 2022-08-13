using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Entities
{
    internal class Customer : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Company { get; set; }
        public string? CompanyName { get; set; }
        public string? NIP { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public Guid UserId { get; set; }
    }
}
