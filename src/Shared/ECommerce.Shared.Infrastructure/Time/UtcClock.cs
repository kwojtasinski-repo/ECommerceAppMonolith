using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Shared.Infrastructure.Time
{
    public class UtcClock : IClock
    {
        public DateTime CurrentDate() => DateTime.UtcNow;
    }
}