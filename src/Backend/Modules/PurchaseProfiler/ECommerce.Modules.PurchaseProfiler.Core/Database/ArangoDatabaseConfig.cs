namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    internal sealed class ArangoDatabaseConfig
    {
        public string Url { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ArgumentNullException("Url to database shouldnt be empty");
            }
        }
    }
}
