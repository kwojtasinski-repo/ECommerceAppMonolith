using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.DTO
{
    internal class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Dictionary<string, IEnumerable<string>> Claims { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
