namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    internal interface IUserApiClient
    {
        Task<GetUserResponse?> GetUser(Guid userId);
    }
}
