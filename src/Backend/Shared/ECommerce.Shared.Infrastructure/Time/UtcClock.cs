using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Shared.Infrastructure.Time
{
    internal class UtcClock : IClock
    {
        public DateTime CurrentDate() => DateTime.UtcNow;
    }
}