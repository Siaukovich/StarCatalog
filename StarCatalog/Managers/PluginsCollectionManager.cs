using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StarCatalog
{
    public static class PluginsCollectionManager
    {
        public static Dictionary<string, IPluginable> Plugins;

        public static async Task LoadPluginsAsync()
        {
            await Task.Delay(2000);

            var currentDirectory = Directory.GetCurrentDirectory();
            var location = Path.Combine(currentDirectory.Substring(0, currentDirectory.Length - "Debug\\bin\\".Length), "Plugins");

            if (!Directory.Exists(location))
            {
                Plugins = new Dictionary<string, IPluginable>();
                throw new DirectoryNotFoundException("Directory for plugins not found!");
            }

            var dllNames = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
            var assemblies = new List<Assembly>(dllNames.Length);
            foreach (var dllName in dllNames)
            {
                var assemblyName = AssemblyName.GetAssemblyName(dllName);
                var assembly = Assembly.Load(assemblyName);
                assemblies.Add(assembly);
            }

            var pluginType = typeof(IPluginable);
            var plugins = assemblies.SelectMany(assembly => assembly.GetTypes())
                                    .Where(type => !type.IsInterface && !type.IsAbstract)
                                    .Where(type => type.GetInterface(pluginType.FullName) != null)
                                    .Select(type => Activator.CreateInstance(type) as IPluginable);

            Plugins = plugins.ToDictionary(p => p.Name);
        }
    }
}
