using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Postgres
{
    public abstract class PostgresUnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _dbContext;

        public PostgresUnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
