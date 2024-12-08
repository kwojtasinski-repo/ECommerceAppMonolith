using ArangoDBNetStandard;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal sealed class UserRepository(IArangoDBClient dBClient, ILogger<UserRepository> logger)
        : IUserRepository
    {
        private const string collectionName = "users";

        public async Task<User> AddAsync(User user)
        {
            var response = await dBClient.Document.PostDocumentAsync(collectionName, user);
            if (!long.TryParse(response._key, out var key))
            {
                logger.LogError("User's key: {key} cannot parse to long", key);
            }
            user.Id = key;
            return user;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return (await dBClient.Document.DeleteDocumentAsync(collectionName, id.ToString())).Old is null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = string.Format("FOR u IN {0} FILTER u.email == @email RETURN u", collectionName);
            var bindVars = new Dictionary<string, object> { { "email", email } };
            var result = await dBClient.Cursor.PostCursorAsync<User>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            var query = string.Format("FOR u IN {0} FILTER u._key == @id RETURN u", collectionName);
            var bindVars = new Dictionary<string, object> { { "id", id.ToString() } };
            var response = await dBClient.Cursor.PostCursorAsync<User>(query, bindVars);
            var user = response.Result.FirstOrDefault();
            if (user is not null)
            {
                user.Id = id;
            }

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            return (await dBClient.Document.PutDocumentAsync(collectionName, user)).New;
        }
    }
}
