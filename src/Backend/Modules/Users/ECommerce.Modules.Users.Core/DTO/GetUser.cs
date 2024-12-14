namespace ECommerce.Modules.Users.Core.DTO
{
    public record GetUser(Guid UserId);

    public record GetUserResponse(Guid UserId, string Email, bool IsActive);
}
