using ArangoDBNetStandard;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal sealed class UserRepository(IArangoDBClient dBClient)
        : IUserRepository
    {
        private const string collectionName = "users";

        public async Task<User> AddAsync(User user)
        {
            return (await dBClient.Document.PostDocumentAsync(collectionName, user)).New;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return (await dBClient.Document.DeleteDocumentAsync(collectionName, id)).Old is null;
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
            var query = string.Format("FOR u IN {0} FILTER u._id == @id RETURN u", collectionName);
            var bindVars = new Dictionary<string, object> { { "id", id } };
            var result = await dBClient.Cursor.PostCursorAsync<User>(query, bindVars);
            return result.Result.FirstOrDefault();
        }

        public async Task<User> UpdateAsync(User user)
        {
            return (await dBClient.Document.PutDocumentAsync(collectionName, user)).New;
        }
    }
}
