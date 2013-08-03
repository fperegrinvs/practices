namespace MTO.Practices.Common.MEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Loader de plugins do MEF
    /// </summary>
    public static class PluginLoader
    {
        /// <summary>
        /// Carrega plugins para o aggregador de plugins definido
        /// </summary>
        /// <param name="obj">O repositorio de plugins com o criterio de importação definido</param>
        /// <param name="pluginsFolder">Diretório onde devemos buscar plugins (caminho absoluto)</param>
        /// <param name="pattern">A extensão dos plugins (default = "*.dll")</param>
        public static void LoadPluginsFor(PluginBag obj, string pluginsFolder = null, string pattern = "*.dll")
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();


            var files = Directory.GetFiles(pluginsFolder ?? AppDomain.CurrentDomain.BaseDirectory, pattern, SearchOption.AllDirectories)
                .Distinct()
                .Where(x => !x.Contains("/obj/") && !x.Contains("\\obj\\"))
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var asmCat = new AssemblyCatalog(file);

                    //Force MEF to load the plugin and figure out if there are any exports
                    // good assemblies will not throw the RTLE exception and can be added to the catalog
                    if (asmCat.Parts.ToList().Count > 0)
                        catalog.Catalogs.Add(asmCat);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var msg = "Erro ao carregar assembly com dependencias não satisfeitas: " + file + "\r\n"
                              + string.Join("\r\n", ex.LoaderExceptions.Select(x => x.ToString()));
                    Logger.Instance.LogEvent(msg);
                }
                catch (BadImageFormatException)
                {
                    Logger.Instance.LogEvent("Erro ao tentar carregar assembly supostamente não .net: " + file);
                }
            }

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(obj);
        }
    }
}
