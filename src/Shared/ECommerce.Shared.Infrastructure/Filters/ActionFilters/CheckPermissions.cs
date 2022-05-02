using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Shared.Infrastructure.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CheckPermissions : Attribute, IAuthorizationFilter
    {
        private readonly PermissionsFilter _permissionsFilter;

        public CheckPermissions(string permission) : this(new PermissionsFilter(permission))
        {
        }

        public CheckPermissions(string[] permissions) : this(new PermissionsFilter(permissions))
        {
        }

        private CheckPermissions(PermissionsFilter permissionsFilter)
        {
            _permissionsFilter = permissionsFilter;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionsFilter.OnAuthorization(context);
        }
    }
}
