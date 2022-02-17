using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure.Messaging.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Messaging.Brokers
{
    internal sealed class InMemoryMessageBroker : IMessageBroker
    {
        private readonly IModuleClient _moduleClient;
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;
        private readonly MessagingOptions _messagingOptions;

        public InMemoryMessageBroker(IModuleClient moduleClient, IAsyncMessageDispatcher asyncMessageDispatcher, MessagingOptions messagingOptions)
        {
            _moduleClient = moduleClient;
            _asyncMessageDispatcher = asyncMessageDispatcher;
            _messagingOptions = messagingOptions;
        }

        public async Task PublishAsync(params IMessage[] messages)
        {
            if (messages is null)
            {
                return;
            }

            messages = messages.Where(m => m is not null).ToArray();

            var tasks = new List<Task>();

            foreach (var message in messages)
            {
                if (_messagingOptions.UseBackgroundDispatcher)
                {
                    await _asyncMessageDispatcher.PublishAsync(message);
                    continue;
                }

                tasks.Add(_moduleClient.PublishAsync(message));
            }

            await Task.WhenAll(tasks);
        }
    }
}
