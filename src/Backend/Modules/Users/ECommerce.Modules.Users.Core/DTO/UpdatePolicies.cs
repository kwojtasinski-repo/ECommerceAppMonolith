using System.ComponentModel.DataAnnotations;

namespace ECommerce.Modules.Users.Core.DTO
{
    internal class UpdatePolicies
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Role { get; set; }

        public IEnumerable<string>? Claims { get; set; }
    }
}
