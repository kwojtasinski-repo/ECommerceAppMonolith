using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Modules
{
    internal sealed class ModuleRegistry : IModuleRegistry
    {
        private readonly ConcurrentBag<ModuleBroadcastRegistration> _broadcastRegistrations = new();
        private readonly ConcurrentDictionary<string, ModuleRequestRegistration> _requestRegistrations = new();

        public void AddBroadcastAction(Type requestType, Func<object, Task> action)
        {
            if (string.IsNullOrWhiteSpace(requestType.Namespace))
            {
                throw new InvalidOperationException("Missing namespace.");
            }

            var registration = new ModuleBroadcastRegistration(requestType, action);
            _broadcastRegistrations.Add(registration);
        }

        public void AddRequestAction(string path, Type requestType, Type responseType, Func<object, Task<object>> action)
        {
            if (path is null)
            {
                throw new InvalidOperationException("Request path cannot be null.");
            }

            var registration = new ModuleRequestRegistration(requestType, responseType, action);
            _requestRegistrations.TryAdd(path, registration);
        }

        public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key)
            => _broadcastRegistrations.Where(t => t.Key == key);

        public ModuleRequestRegistration GetRequestRegistration(string path)
            => _requestRegistrations.TryGetValue(path, out var registration) ? registration : null;
    }

}
