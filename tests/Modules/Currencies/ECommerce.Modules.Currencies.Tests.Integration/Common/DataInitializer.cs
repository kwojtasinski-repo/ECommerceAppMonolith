using ECommerce.Modules.Currencies.Core.DAL;
using ECommerce.Modules.Currencies.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    internal class DataInitializer(CurrenciesDbContext dbContext) 
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var currencyPln = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "PLN", cancellationToken: cancellationToken)
               ?? new Currency { Id = Guid.NewGuid(), Code = "PLN", Description = "Polski złoty" };
            var currencyEur = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "EUR", cancellationToken: cancellationToken)
                ?? new Currency { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro" };
            var currencyUsd = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "USD", cancellationToken: cancellationToken)
                ?? new Currency { Id = Guid.NewGuid(), Code = "USD", Description = "Dolar amerykański" };
            var currencyChf = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "CHF", cancellationToken: cancellationToken)
                ?? new Currency { Id = Guid.NewGuid(), Code = "CHF", Description = "Frank szwajcarski" };
            var currencyGbp = await dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "GBP", cancellationToken: cancellationToken)
                ?? new Currency { Id = Guid.NewGuid(), Code = "GBP", Description = "Funt brytyjski" };

            var currencyDate = DateOnly.FromDateTime(DateTime.UtcNow);
            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyPln.Id && cr.CurrencyDate == currencyDate, cancellationToken: cancellationToken))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyPln, CurrencyDate = currencyDate, CurrencyId = currencyPln.Id, Rate = decimal.One }, cancellationToken: cancellationToken);
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyEur.Id && cr.CurrencyDate == currencyDate, cancellationToken: cancellationToken))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyEur, CurrencyDate = currencyDate, CurrencyId = currencyEur.Id, Rate = 4.2512M }, cancellationToken: cancellationToken);
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyUsd.Id && cr.CurrencyDate == currencyDate, cancellationToken: cancellationToken))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyUsd, CurrencyDate = currencyDate, CurrencyId = currencyUsd.Id, Rate = 2.5123M }, cancellationToken: cancellationToken);
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyChf.Id && cr.CurrencyDate == currencyDate, cancellationToken: cancellationToken))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyChf, CurrencyDate = currencyDate, CurrencyId = currencyChf.Id, Rate = 3.5632M }, cancellationToken: cancellationToken);
            }

            if (!await dbContext.CurrencyRates.AnyAsync(cr => cr.Id == currencyGbp.Id && cr.CurrencyDate == currencyDate, cancellationToken: cancellationToken))
            {
                await dbContext.CurrencyRates.AddAsync(new CurrencyRate { Id = Guid.NewGuid(), Currency = currencyGbp, CurrencyDate = currencyDate, CurrencyId = currencyGbp.Id, Rate = 5.2645M }, cancellationToken: cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
