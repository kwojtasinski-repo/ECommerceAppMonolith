using System.ComponentModel.DataAnnotations;

namespace ECommerce.Modules.Users.Core.DTO
{
    internal class ChangeCredentialsDto
    {
        [EmailAddress]
        [Required]
        public string OldEmail { get; set; }
        [EmailAddress]
        public string NewEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
