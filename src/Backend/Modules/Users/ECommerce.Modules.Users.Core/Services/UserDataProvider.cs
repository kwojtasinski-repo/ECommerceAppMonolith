using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Mappings;
using ECommerce.Modules.Users.Core.Repositories;

namespace ECommerce.Modules.Users.Core.Services
{
    internal sealed class UserDataProvider
        (
            IUserRepository userRepository
        )
        : IUserDataProvider
    {
        public async Task<UserDataDto?> GetUserDataAsync(GetUserDataDto dto)
        {
            return (await userRepository.GetUserDataAsync(dto.UserId))?.AsUserData();
        }
    }
}
