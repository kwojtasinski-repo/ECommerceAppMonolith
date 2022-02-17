using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Messaging.Dispatchers
{
    internal sealed class AsyncMessageDispatcher : IAsyncMessageDispatcher
    {
        private readonly IMessageChannel _messageChannel;

        public AsyncMessageDispatcher(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel;
        }

        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
            => _messageChannel.Writer.WriteAsync(message);
    }
}
