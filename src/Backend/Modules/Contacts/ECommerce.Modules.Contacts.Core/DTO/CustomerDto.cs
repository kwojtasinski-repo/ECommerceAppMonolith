using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DTO
{
    internal class CustomerDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool Company { get; set; }
        public string? CompanyName { get; set; }
        public string? NIP { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
