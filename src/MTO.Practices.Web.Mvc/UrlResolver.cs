namespace MTO.Practices.Common
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Faz a resolução de Urls para links gerados pelo CMS
    /// </summary>
    public class UrlResolver
    {
        /// <summary>
        /// Caminho para o handler de arquivos
        /// </summary>
        private const string FileHandlerPath = "arquivo/";

        /// <summary>
        /// Caracteres de trimming de caminho
        /// </summary>
        private static readonly char[] PathTrim = new[] { '/', '~' };

        /// <summary>
        /// Domínio transacional (fora do akamai)
        /// </summary>
        private static readonly string TransactionalDomain = ConfigurationManager.AppSettings["TransactionalDomain"];

        /// <summary>
        /// Domínio estático, pode estar atrás de akamai
        /// </summary>
        private static readonly string StaticDomain = AddProtocol(ConfigurationManager.AppSettings["StaticDomain"]);

        /// <summary>
        /// Regex de urls em css
        /// </summary>
        private static readonly Regex CssUrlRegex = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Delegates de transformação de url a serem aplicados com UrlResolver
        /// </summary>
        /// <param name="url">
        /// A url a ser transformada
        /// </param>
        /// <returns>A url transformada</returns>
        public delegate string UrlTransformation(string url);

        /// <summary>
        /// Delegates de seleção de domínio capazes de decidir o domínio com base no relativePath
        /// </summary>
        /// <param name="staticDomain">O domínio estático</param>
        /// <param name="transactionalDomain">O dómínio dinâmico</param>
        /// <param name="relativePath">O caminho relativo da url</param>
        /// <returns>O Domínio selecionado</returns>
        public delegate string DomainSelection(string staticDomain, string transactionalDomain, string relativePath);

        /// <summary>
        /// Resolvedores de Diretório
        /// </summary>
        /// <param name="pathId">Id do diretório</param>
        /// <returns>Caminho do diretório</returns>
        public delegate string PathResolution(Guid pathId);

        /// <summary>
        /// Os métodos de transformação de url registrados
        /// </summary>
        private static UrlTransformation UrlTransformations { get; set; }

        /// <summary>
        /// Os métodos de resolução de path registrados
        /// </summary>
        private static PathResolution PathResolvers { get; set; }

        /// <summary>
        /// Os metodos de seleção de domínio registrados
        /// </summary>
        private static DomainSelection DomainSelectors { get; set; }

        /// <summary>
        /// Registra uma transformação de url
        /// </summary>
        /// <param name="transform">A função de transformação</param>
        public static void RegisterUrlTransformation(UrlTransformation transform)
        {
            UrlTransformations += transform;
        }

        /// <summary>
        /// Registra uma transformação de url
        /// </summary>
        /// <param name="resolver">A função de resolução de caminho</param>
        public static void RegisterPathResolver(PathResolution resolver)
        {
            PathResolvers += resolver;
        }

        /// <summary>
        /// Registra um seletor de domínio
        /// </summary>
        /// <param name="selector">A função seletora de domínio</param>
        public static void RegisterDomainSelector(DomainSelection selector)
        {
            DomainSelectors += selector;
        }

        /// <summary>
        /// Resolve um caminho relativo arbitrário no domínio estático
        /// </summary>
        /// <param name="relativePath">Caminho relativo</param>
        /// <param name="staticDomain">Utilizar domínio estático (default=true)</param>
        /// <returns>Retorna a url relativa no domínio requisitado</returns>
        public static string ResolveUrl(string relativePath, bool staticDomain = true)
        {
            var url = staticDomain ? StaticDomain : TransactionalDomain;

            if (DomainSelectors != null)
            {
                foreach (DomainSelection select in DomainSelectors.GetInvocationList())
                {
                    // Se o seletor não retorna nada, é porque devemos usar nossa própria regra
                    url = select(StaticDomain, TransactionalDomain, relativePath) ?? url;
                }
            }

            // Passamos o controle a possíveis operações de transformação de Url
            if (UrlTransformations != null)
            {
                foreach (UrlTransformation transform in UrlTransformations.GetInvocationList())
                {
                    relativePath = transform(relativePath);
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                url = GetUrlHelper().Content(relativePath);

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("Não foi possível resolver a url do handler de arquivos.");
                }

                return url;
            }

            return JoinPaths(url, relativePath);
        }

        /// <summary>
        /// Resolve a url para um arquivo
        /// </summary>
        /// <param name="fileId">Arquivo a ser resolvido</param>
        /// <returns>Url resolvida</returns>
        public static string ResolveFile(string fileId)
        {
            var url = StaticDomain;

            // Tentamos usar o UrlHelper.. axo q em prod por causa do Balance isso não vai funcionar
            if (string.IsNullOrEmpty(url))
            {
                url = GetUrlHelper().Action("Render", "File", new { id = fileId });

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("Não foi possível resolver a url do handler de arquivos.");
                }

                return url;
            }

            if (url.EndsWith("/"))
            {
                return JoinPaths(url, FileHandlerPath + fileId);
            }

            return JoinPaths(url, FileHandlerPath + fileId);
        }

        /// <summary>
        /// Constroi um UrlHelper para uso dentro da biblioteca
        /// </summary>
        /// <returns>O UrlHelper</returns>
        public static UrlHelper GetUrlHelper()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            return new UrlHelper(new RequestContext(httpContext, CurrentRoute(httpContext)));
        }

        /// <summary>
        /// Resolve urls presentes no texto com base no diretório informado, caso seja informado
        /// </summary>
        /// <param name="text">O texto onde buscar as urls</param>
        /// <param name="pathId">O id do caminho base, caso haja</param>
        /// <returns>Texto com as urls resolvidas</returns>
        public static string ResolveUrlsInCss(string text, Guid? pathId = null)
        {
            if (pathId.HasValue && !PathResolvers.GetInvocationList().Any())
            {
                Logger.Instance.LogError(new Exception("Nenhum Path Resolver registrado, impossível resolver PathId"));
                return text;
            }

            var basePath = string.Empty;
            if (pathId.HasValue)
            {
                // Chamamos os pathResolvers, que rodam em camadas abaixo da aplicação cacheando diretórios
                basePath = PathResolvers(pathId.Value);

                if (basePath == null)
                {
                    Logger.Instance.LogError(new Exception("Nenhum caminho encontrado para o PathId: " + pathId.Value));
                    return text;
                }
            }

            return ResolveUrlsInCss(text, basePath);
        }

        /// <summary>
        /// Resolve urls presentes no texto com base no diretório informado
        /// </summary>
        /// <param name="text">O texto onde buscar as urls</param>
        /// <param name="basePath">Caminho base a ser usado na resolução de caminhos relativos</param>
        /// <returns>Texto com as urls resolvidas</returns>
        public static string ResolveUrlsInCss(string text, string basePath)
        {
            return CssUrlRegex.Replace(
                text,
                m =>
                {
                    var relativeUrl = m.Groups[2].Value;

                    var absoluteUrl = ToAbsolutePath(relativeUrl, basePath);

                    var urlReplace = ResolveUrl(absoluteUrl);
                    return string.Format("url({0}{1}{0})", m.Groups[1].Value, urlReplace);
                });
        }

        /// <summary>
        /// Converte caminho relativo em caminho absoluto com base no basePath informado
        /// </summary>
        /// <param name="relativeUrl">Caminho relativo</param>
        /// <param name="basePath">Caminho atual em formato absoluto</param>
        /// <returns>O caminho relativo transformado em absoluto</returns>
        private static string ToAbsolutePath(string relativeUrl, string basePath)
        {
            // Se a url não é absoluta
            if (!relativeUrl.StartsWith("/") && !relativeUrl.StartsWith("http") && !relativeUrl.StartsWith("[Repositorio]") && !relativeUrl.StartsWith("%5BRepositorio%5D"))
            {
                if (!basePath.EndsWith("/"))
                {
                    basePath += "/";
                }

                if (!basePath.StartsWith("/"))
                {
                    basePath = "/" + basePath;
                }

                while (relativeUrl.StartsWith("../"))
                {
                    relativeUrl = relativeUrl.Remove(0, 3);

                    // matamos a ultima /
                    var idx = basePath.LastIndexOf('/');

                    if (idx == 0)
                    {
                        // Tem mais ../ do que caminhos no basePath.. alguém exagerou nos ../ pra jogar seguro, possivelmente
                        continue;
                    }

                    basePath = basePath.Substring(0, idx);

                    // deixamos a penultima
                    idx = basePath.LastIndexOf('/');
                    basePath = basePath.Substring(0, idx + 1);
                }

                relativeUrl = basePath + relativeUrl;
            }

            return relativeUrl;
        }

        /// <summary>
        /// Recupera as informações de roteamento do contexto http atual
        /// </summary>
        /// <param name="httpContext">O HttpContext atual</param>
        /// <returns>As informações de roteamento</returns>
        private static RouteData CurrentRoute(HttpContextWrapper httpContext)
        {
            return RouteTable.Routes.GetRouteData(httpContext);
        }

        /// <summary>
        /// Junta dois paths de url
        /// </summary>
        /// <param name="path1">Path um</param>
        /// <param name="path2">Path dois</param>
        /// <returns>Caminhos concatenados</returns>
        private static string JoinPaths(string path1, string path2)
        {
            return path1.TrimEnd('/') + '/' + path2.TrimStart(PathTrim);
        }

        /// <summary>
        /// Adiciona protocolo na url
        /// </summary>
        /// <param name="url">A url</param>
        /// <returns>A url com protocolo</returns>
        private static string AddProtocol(string url)
        {
            var protocol = url.StartsWith("htt") ? string.Empty : "http://";
            return protocol + url;
        }
    }
}
