using ECommerce.Shared.Abstractions.Messagging;
using ECommerce.Shared.Infrastructure.Messaging.Brokers;
using ECommerce.Shared.Infrastructure.Messaging.Dispatchers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Messaging
{
    internal static class Extensions
    {
        private const string SectionName = "messaging";

        internal static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();

            var messagingOptions = services.GetOptions<MessagingOptions>(SectionName);
            services.AddSingleton(messagingOptions);

            if (messagingOptions.UseBackgroundDispatcher)
            {
                services.AddHostedService<BackgroundDispatcher>();
            }
            return services;
        }
    }
}
