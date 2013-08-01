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
        /// <param name="obj"></param>
        /// <param name="pattern"></param>
        public static void LoadPluginsFor(PluginBag obj, string pattern = null)
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            var curPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var cats = SafeDirectoryCatalog(curPath, pattern);
            foreach (var assemblyCatalog in cats)
            {
                catalog.Catalogs.Add(assemblyCatalog);
            }

            //Adds all the parts found in all assemblies in 
            //the same directory as the executing program
            string[] folders = System.IO.Directory.GetDirectories(curPath, "*", System.IO.SearchOption.AllDirectories);

            foreach (string folder in folders)
            {
                cats = SafeDirectoryCatalog(folder, pattern);

                foreach (var assemblyCatalog in cats)
                {
                    catalog.Catalogs.Add(assemblyCatalog);
                }
            }

            //Create the CompositionContainer with the parts in the catalog
            var container = new CompositionContainer(catalog);

            //Fill the imports of this object
            container.ComposeParts(obj);
        }

        private static IEnumerable<AssemblyCatalog> SafeDirectoryCatalog(string directory, string pattern = null)
        {
            var files = Directory.EnumerateFiles(directory, pattern ?? "*.dll", SearchOption.AllDirectories);

            var result = new List<AssemblyCatalog>();

            foreach (var file in files)
            {
                try
                {
                    var asmCat = new AssemblyCatalog(file);

                    //Force MEF to load the plugin and figure out if there are any exports
                    // good assemblies will not throw the RTLE exception and can be added to the catalog
                    if (asmCat.Parts.ToList().Count > 0)
                        result.Add(asmCat);
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

            return result;
        }
    }
}
