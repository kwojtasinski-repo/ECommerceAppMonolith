namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KeyGenerationTypeAttribute : Attribute
    {
        public KeyGenerationType KeyGenerationType { get; }

        public KeyGenerationTypeAttribute(KeyGenerationType keyGenerationType)
        {
            KeyGenerationType = keyGenerationType;
        }
    }

    public enum KeyGenerationType
    {
        Traditional,
        Autoincrement,
        Uuid,
        Padded
    }
}
