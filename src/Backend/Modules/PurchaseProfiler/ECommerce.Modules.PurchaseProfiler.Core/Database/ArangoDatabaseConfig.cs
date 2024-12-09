namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    internal sealed class ArangoDatabaseConfig
    {
        public string Url { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RootUserName { get; set; } = string.Empty;
        public string RootPassword { get; set; } = string.Empty;
        public bool InitializeDatabaseOnStart { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ArgumentNullException("Url to database shouldn't be empty");
            }

            if (InitializeDatabaseOnStart)
            {
                if (string.IsNullOrWhiteSpace(Database))
                {
                    throw new ArgumentNullException("Database field shouldn't be empty");
                }

                if (string.IsNullOrWhiteSpace(RootUserName))
                {
                    throw new ArgumentNullException("RootUserName field shouldn't be empty");
                }

                if (string.IsNullOrWhiteSpace(RootPassword))
                {
                    throw new ArgumentNullException("RootPassword field shouldn't be empty");
                }
            }
        }
    }
}
