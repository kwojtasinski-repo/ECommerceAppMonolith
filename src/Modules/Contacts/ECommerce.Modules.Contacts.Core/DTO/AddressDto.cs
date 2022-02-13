using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Contacts.Core.DTO
{
    internal class AddressDto
    {
        public Guid Id { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string BuildingNumber { get; set; }
        public string? LocaleNumber { get; set; }
        public Guid CustomerId { get; set; }
    }
}
