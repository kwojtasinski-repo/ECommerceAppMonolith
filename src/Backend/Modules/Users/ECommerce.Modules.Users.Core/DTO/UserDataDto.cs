namespace ECommerce.Modules.Users.Core.DTO
{
    public record UserDataDto(Guid UserId, string Email, bool IsActive, DateTime CreatedAt);
}
