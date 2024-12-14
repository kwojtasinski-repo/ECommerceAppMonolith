using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;

namespace ECommerce.Modules.Users.Core.Mappings
{
    internal static class Extensions
    {
        public static AccountDto AsAccountDto(this User user)
        {
            return new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Claims = user.Claims ?? [],
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public static UserDataDto AsUserData(this User user)
        {
            return new UserDataDto(user.Id, user.Email, user.IsActive, user.CreatedAt);
        }
    }
}
