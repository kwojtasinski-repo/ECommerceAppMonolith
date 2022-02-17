using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Abstractions.Modules
{
    public interface IModuleClient
    {
        Task SendAsync(string path, object request);
        Task<TResult> SendAsync<TResult>(string path, object request) where TResult : class;
        Task PublishAsync(object message);
    }
}
