namespace ECommerce.Modules.Currencies.Core.Clients.External
{
    internal class ExchangeRateTable
    {
        public string Table { get; set; }
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }

        public List<RateTable> Rates { get; set; }
    }
}
