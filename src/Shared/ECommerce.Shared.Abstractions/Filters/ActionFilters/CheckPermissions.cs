using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Shared.Abstractions.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CheckPermissions : Attribute, IAuthorizationFilter
    {
        private readonly IEnumerable<string> _permissions;

        public CheckPermissions(string permission) : this(new string [] { permission })
        {
        }

        public CheckPermissions(IEnumerable<string> permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Claims.GroupBy(c => c.Type)
                .Where(c => c.Key == "permissions")
                .ToDictionary(c => c.Key, x => x.Select(c => c.Value.ToString()));

            var permissionClaim = claims
                                    .Where(c => c.Value.Any(claim => _permissions.Contains(claim)))
                                    .Select(c => c.Value);

            if (!permissionClaim.Any())
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
