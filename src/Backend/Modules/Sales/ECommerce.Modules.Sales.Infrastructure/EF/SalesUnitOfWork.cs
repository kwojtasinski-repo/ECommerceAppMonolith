using ECommerce.Shared.Infrastructure.Postgres;

namespace ECommerce.Modules.Sales.Infrastructure.EF
{
    internal class SalesUnitOfWork : PostgresUnitOfWork<SalesDbContext>, ISalesUnitOfWork
    {
        public SalesUnitOfWork(SalesDbContext dbContext) : base(dbContext)
        {
        }
    }
}
