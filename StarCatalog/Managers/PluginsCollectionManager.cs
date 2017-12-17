using System;
using System.Collections.Generic;
using System.Configuration;
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

            const string pluginFolderConfigKey = "PluginsFolder";
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(pluginFolderConfigKey))
            {
                throw new ConfigurationErrorsException("Configuration doesn't contain value for plugin folder!");
            }

            string pluginsFolderName = ConfigurationManager.AppSettings[pluginFolderConfigKey];
            string pluginsDirectory = Path.Combine(Environment.CurrentDirectory, pluginsFolderName);
            if (!Directory.Exists(pluginsDirectory))
            {
                Plugins = new Dictionary<string, IPluginable>();
                Directory.CreateDirectory(pluginsDirectory);
                throw new DirectoryNotFoundException("Directory for plugins was not found! Directory created.");
            }

            var dllNames = Directory.GetFiles(pluginsDirectory, "*.dll", SearchOption.AllDirectories);
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
