namespace ECommerce.Modules.Users.Core.DTO
{
    internal class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Dictionary<string, IEnumerable<string>> Claims { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
