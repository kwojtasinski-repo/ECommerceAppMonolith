using ECommerce.Shared.Abstractions.Messagging;
using System.Threading.Channels;

namespace ECommerce.Shared.Infrastructure.Messaging.Dispatchers
{
    internal interface IMessageChannel
    {
        ChannelReader<IMessage> Reader { get; }
        ChannelWriter<IMessage> Writer { get; }
    }
}
