namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    public record GetUser(Guid UserId);

    public record GetUserResponse(Guid UserId, string Email, bool IsActive);
}
