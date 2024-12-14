using ECommerce.Modules.Users.Core.DTO;

namespace ECommerce.Modules.Users.Core.Services
{
    public interface IUserDataProvider
    {
        Task<UserDataDto?> GetUserDataAsync(GetUserDataDto dto);
    }
}
