using ECommerce.Modules.Users.Core.DTO;
using ECommerce.Modules.Users.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.Mappgins
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
                Claims = user.Claims,
                CreatedAt = user.CreatedAt
            };

            return dto;
        }
    }
}
