using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.Entities
{
    internal class AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? InactivedBy { get; set; }
        public DateTime? Inactived { get; set; }
        public bool Active { get; set; }
    }
}
