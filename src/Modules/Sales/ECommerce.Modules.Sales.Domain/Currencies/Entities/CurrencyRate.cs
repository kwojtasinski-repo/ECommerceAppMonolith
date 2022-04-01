namespace ECommerce.Modules.Sales.Domain.Currencies.Entities
{
    public class CurrencyRate
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
        public string CurrencyCode { get; set; }
        public DateOnly RateDate { get; set; }
        public DateOnly Created { get; set; }
    }
}
