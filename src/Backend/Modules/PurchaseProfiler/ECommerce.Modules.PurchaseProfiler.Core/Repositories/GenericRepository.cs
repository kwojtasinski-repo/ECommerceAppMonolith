using ArangoDBNetStandard;
using ECommerce.Modules.PurchaseProfiler.Core.Database;
using ECommerce.Modules.PurchaseProfiler.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.PurchaseProfiler.Core.Repositories
{
    internal class GenericRepository<T, U>
        (
            IArangoDBClient dbClient,
            DbConfiguration dbConfiguration,
            ILogger<GenericRepository<T,U>> logger
        )
        : IGenericRepository<T, U>
        where T : class, IDocumentEntity<U>
        where U : struct
    {
        public string CollectionName => dbConfiguration.GetCollectionName(typeof(T));
        public IArangoDBClient DbClient => dbClient;

        public async Task<T> AddAsync(T entity)
        {
            var response = await DbClient.Document.PostDocumentAsync(CollectionName, entity);
            entity.Key = response._key;
            entity.Id = response._id;
            return entity;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                await DbClient.Document.DeleteDocumentAsync(CollectionName, key);
                return true;
            }
            catch (ApiErrorException exception)
            {
                if (exception.ApiError.Code == System.Net.HttpStatusCode.NotFound)
                {
                    logger.LogWarning("Entity with key '{key}' was not found", key);
                    return false;
                }

                logger.LogError(exception, "There was an error while deleting entity with key '{key}', error status code '{statusCode}', message: {message}", key, exception.ApiError.ErrorNum, exception.ApiError.ErrorMessage);
                throw;
            }
        }

        public async Task<T?> GetByKeyAsync(string key)
        {
            var query = string.Format("FOR u IN {0} FILTER entity._key == @key RETURN entity", CollectionName);
            var bindVars = new Dictionary<string, object> { { "key", key } };
            var response = await DbClient.Cursor.PostCursorAsync<T>(query, bindVars);
            if (response is null || response.Error)
            {
                logger.LogError("There was an error while getting collection '{collection}' with key '{key}', status code: '{statusCode}'", CollectionName, key, (int)(response?.Code ?? 0));
                return null;
            }

            var entity = response.Result.FirstOrDefault();
            return entity;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            try
            {
                await DbClient.Document.PutDocumentAsync(CollectionName, entity.Key, entity);
                return entity;
            }
            catch (ApiErrorException exception)
            {
                if (exception.ApiError.Code == System.Net.HttpStatusCode.NotFound)
                {
                    logger.LogWarning("Entity with key '{key}' was not found", entity.Key);
                    return null;
                }

                logger.LogError(exception, "There was an error while deleting entity with key '{key}', error status code '{statusCode}', message: {message}", entity.Key, exception.ApiError.ErrorNum, exception.ApiError.ErrorMessage);
                throw;
            }
        }
    }
}
