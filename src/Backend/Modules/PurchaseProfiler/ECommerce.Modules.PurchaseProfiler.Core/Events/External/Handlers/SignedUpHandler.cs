using ECommerce.Modules.PurchaseProfiler.Core.Repositories;
using ECommerce.Modules.Sales.Application.Items.Events.External;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External.Handlers
{
    internal sealed class SignedUpHandler : IEventHandler<SignedUp>
    {
        private readonly ILogger<SignedUpHandler> _logger;
        private readonly IUserRepository _userRepository;

        public SignedUpHandler(ILogger<SignedUpHandler> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            _logger.LogInformation("Received event: {eventName}, value: {eventValue}", nameof(SignedUp), JsonSerializer.Serialize(@event));
            try
            {
                await _userRepository.AddAsync(new Entities.User
                {
                    UserId = @event.UserId,
                    Email = @event.Email,
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{handler}: There was an error", nameof(SignedUpHandler));
            }
        }
    }
}
