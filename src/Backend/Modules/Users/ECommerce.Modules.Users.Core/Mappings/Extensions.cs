using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;

namespace ECommerce.Modules.Users.Core.Mappings
{
    internal static class Extensions
    {
        public static AccountDto AsAccountDto(this User user)
        {
            var dto = new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Claims = user.Claims ?? [],
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };

            return dto;
        }
    }
}
