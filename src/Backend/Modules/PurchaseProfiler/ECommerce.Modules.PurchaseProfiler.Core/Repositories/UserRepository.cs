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
            user.Key = response._key;
            user.Id = response._id;
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                await dBClient.Document.DeleteDocumentAsync(collectionName, id);
                return true;
            }
            catch (ApiErrorException)
            {
                return false;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = string.Format("FOR u IN {0} FILTER u.email == @email RETURN u", collectionName);
            var bindVars = new Dictionary<string, object> { { "email", email } };
            var result = await dBClient.Cursor.PostCursorAsync<User>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            var query = string.Format("FOR u IN {0} FILTER u._key == @id RETURN u", collectionName);
            var bindVars = new Dictionary<string, object> { { "id", id } };
            var response = await dBClient.Cursor.PostCursorAsync<User>(query, bindVars);
            var user = response.Result.FirstOrDefault();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            await dBClient.Document.PutDocumentAsync(collectionName, user.KeyValue.ToString(), user);
            return user;
        }
    }
}
