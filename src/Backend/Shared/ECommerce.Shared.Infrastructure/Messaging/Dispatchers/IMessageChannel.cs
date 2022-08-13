using ECommerce.Shared.Abstractions.Messagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Messaging.Dispatchers
{
    internal interface IMessageChannel
    {
        ChannelReader<IMessage> Reader { get; }
        ChannelWriter<IMessage> Writer { get; }
    }
}
