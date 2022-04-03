namespace ECommerce.Modules.Currencies.Core.Clients.External
{
    internal interface INbpClient
    {
        Task<ExchangeRate> GetCurrencyAsync(string currencyCode);
        Task<ExchangeRate> GetCurrencyRateOnDateAsync(string currencyCode, DateOnly dateTime);
        Task<IEnumerable<ExchangeRateTable>> GetAllCurrenciesForCurrentDay(CancellationToken cancellationToken);
    }
}
