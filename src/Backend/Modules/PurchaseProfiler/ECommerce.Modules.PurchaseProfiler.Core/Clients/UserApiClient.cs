using ECommerce.Shared.Abstractions.Modules;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients
{
    internal sealed class UserApiClient 
        (
            IModuleClient moduleClient
        )
        : IUserApiClient
    {
        public async Task<GetUserResponse?> GetUser(Guid userId)
        {
            return await moduleClient.SendAsync<GetUserResponse>("/users/get", new GetUser(userId));
        }
    }
}
