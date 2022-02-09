using ECommerce.Shared.Infrastructure.Auth;
using ECommerce.Shared.Infrastructure.Time;

namespace ECommerce.Shared.Tests
{
    public static class AuthHelper
    {
        private static readonly AuthManager AuthManger;

        static AuthHelper()
        {
            var authOptions = OptionsHelper.GetOptions<AuthOptions>("auth");
            AuthManger = new AuthManager(authOptions, new UtcClock());
        }

        public static string GenerateJwt(string userId, string role = null, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
        {
            var token = AuthManger.CreateToken(userId, role, audience, claims);
            return token.AccessToken;
        }
    }
}
