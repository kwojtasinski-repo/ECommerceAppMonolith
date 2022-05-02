using System.ComponentModel.DataAnnotations;

namespace ECommerce.Modules.Users.Core.DTO
{
    public class ChangeUserActive
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
