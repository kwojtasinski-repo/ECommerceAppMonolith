namespace ECommerce.Modules.Currencies.Core.Services
{
    internal interface ICurrencyRateDownloader
    {
        Task Download(CancellationToken cancellationToken = default);
    }
}
