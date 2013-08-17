namespace MTO.Tools.CreatePracticesAssembly.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using MTO.Tools.CreatePracticesAssembly.Models;

    public class HomeController : Controller
    {
        public List<Pacote> GetPacotes()
        {
            var pacotes = new List<Pacote>
                {
                    new Pacote { Id = "cacheCouchbase", Name = "Cache.Couchbase" },
                    new Pacote { Id = "common", Name = "Common" },
                    new Pacote { Id = "commonFunq", Name = "Common.Funq" },
                    new Pacote { Id = "commonUnity", Name = "Common.Unity" },
                    new Pacote { Id = "entity", Name = "Entity" },
                    new Pacote { Id = "excel", Name = "Excel" },
                    new Pacote { Id = "geolocation", Name = "Geolocation" },
                    new Pacote { Id = "elmah", Name = "Logging.Elmah" },
                    new Pacote { Id = "minify", Name = "Minify" },
                    new Pacote { Id = "lexertemplate", Name = "Templating.Lexer" },
                    new Pacote { Id = "stringtemplate", Name = "Templating.StringTemplate" },
                    new Pacote { Id = "valueinjector", Name = "ValueInjector" },
                    new Pacote { Id = "mvc", Name = "Web.Mvc" },
                    new Pacote { Id = "zip", Name = "Zip" },
                };

            return pacotes;
        }

        public ActionResult Index()
        {
            var pacotes = this.GetPacotes();
            return this.View(pacotes);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(List<string> pacote)
        {
            var pacotesDictionary = this.GetPacotes().ToDictionary(c => c.Id);
            var pacotes = (from p in pacote orderby p != "common" select pacotesDictionary[p]).ToList();

            var merge = new ILMerging.ILMerge();
            merge.KeyFile = Server.MapPath("MTO.snk");

            var path = Server.MapPath("/bin/");
            var input = (from p in pacotes select path + "MTO.Practices." + p.Name + ".dll").ToArray();
            merge.SetInputAssemblies(input);
            merge.OutputFile = Server.MapPath("/tmp/") + "MTO.Practices.dll";
            merge.Merge();

            return File(merge.OutputFile, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(merge.OutputFile));
        }
    }
}
