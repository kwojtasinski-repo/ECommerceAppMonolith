using System.ComponentModel.DataAnnotations;

namespace ECommerce.Modules.Contacts.Core.DTO
{
    internal class AddressDto
    {
        public Guid Id { get; set; }
        [Required]
        public string CityName { get; set; } = null!;
        [Required]
        public string StreetName { get; set; } = null!;
        [Required]
        public string CountryName { get; set; } = null!;
        [Required]
        public string ZipCode { get; set; } = null!;
        [Required]
        public string BuildingNumber { get; set; } = null!;
        public string? LocaleNumber { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
    }
}
