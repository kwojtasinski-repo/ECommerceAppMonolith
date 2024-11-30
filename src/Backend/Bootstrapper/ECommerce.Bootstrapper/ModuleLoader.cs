using ECommerce.Shared.Abstractions.Modules;
using ECommerce.Shared.Infrastructure;
using System.Reflection;

namespace ECommerce.Bootstrapper
{
    internal static class ModuleLoader
    {
        public static IList<Assembly> LoadAssemblies(IConfiguration configuration)
        {
            const string modulePart = "ECommerce.Modules.";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var locations = assemblies.Where(a => !a.IsDynamic).Select(l => l.Location).ToList();
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(f => !locations.Contains(f, StringComparer.InvariantCultureIgnoreCase) && f.Contains(modulePart))
                .ToList();

            var disabledModules = new List<string>();

            foreach(var file in files)
            {
                if (!file.Contains(modulePart))
                {
                    continue;
                }

                var moduleName = file.GetModuleName();
                var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");

                if (!enabled)
                {
                    disabledModules.Add(file);
                }
            }

            foreach(var disabledModule in disabledModules)
            {
                files.Remove(disabledModule);
            }

            files.ForEach(f => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(f))));
            return assemblies;
        }

        public static IList<IModule> LoadModules(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetTypes());
            var modules = types.Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface)
                            .OrderBy(t => t.Name)
                            .Select(Activator.CreateInstance)
                            .Cast<IModule>()
                            .ToList();
                            
            return modules;
        }
    }
}
