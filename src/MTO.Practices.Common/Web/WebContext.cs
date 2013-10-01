namespace MTO.Practices.Common.Web
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;

    using MTO.Practices.Common.Crypto;
    using MTO.Practices.Common.Interfaces;

    using IUserStore = MTO.Practices.Common.Interfaces.IUserStore;

    /// <summary>
    /// Gerencia o contexto através de Requests
    /// </summary>
    public class WebContext : IContext
    {
        /// <summary>
        /// Nome do cookie de atividades do CMS
        /// </summary>
        private const string ActivityCookieName = "CMS-Atividades";

        /// <summary>
        /// Senha de encriptação das atividades no cookie
        /// </summary>
        private const string ActivityCookieSecret = "segredo oioi 123";

        /// <summary>
        /// Repositório de verificação de existência e habilitação de usuários
        /// </summary>
        private IUserStore userStore;

        /// <summary>
        /// Credenciais da thread atual
        /// </summary>
        public ConcurrentDictionary<string, ICredential> Credentials
        {
            get
            {
                return this.Get<ConcurrentDictionary<string, ICredential>>("Credentials") ?? new ConcurrentDictionary<string, ICredential>();
            }

            set
            {
                this.Set("Credentials", value);
            }
        }

        /// <summary>
        /// Repositório de verificação de existência e habilitação de usuários
        /// </summary>
        protected IUserStore UserStore
        {
            get
            {
                return this.userStore ?? (this.userStore = Injector.ResolveInterface<IUserStore>());
            }
        }

        /// <summary>
        /// Dicionário de objetos do contexto
        /// </summary>
        protected IDictionary Items
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Items;
                }

                return null;
            }
        }

        /// <summary>
        /// Fallback para ThreadContext, para casos onde chamamos código Threaded utilizando EF
        /// </summary>
        protected ThreadContext Fallback
        {
            get
            {
                return new ThreadContext();
            }
        }

        /// <summary>
        /// Constroi nova instância do contexto
        /// </summary>
        /// <returns>nova instância do contexto</returns>
        public IContext NewInstance()
        {
            return new WebContext();
        }

        /// <summary>
        /// Recupera se o usuário está autenticado, existe no banco e está habilitado
        /// </summary>
        /// <returns>Verdadeiro se o usuário está autenticado</returns>
        public bool IsAuthenticated()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (!this.UserStore.UserExistsAndEnabled(this.GetCurrentUserId()))
            {
                this.LogOff();
                HttpContext.Current.Response.Redirect("/");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Recupera o perfil do usuário logado
        /// </summary>
        /// <returns>Perfil do usuário logado</returns>
        public string GetUserProfile()
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new AuthenticationException("Usuário não autenticado.");
            }

            var parts = identity.Name.Split('_');
            if (parts.Length == 3)
            {
                return parts[1];
            }

            throw new InvalidCredentialException("Contexto de autenticação corrompido.");
        }

        /// <summary>
        /// Recupera o nome do usuário logado
        /// </summary>
        /// <returns>Nome do usuário logado</returns>
        public string GetCurrentUserName()
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new AuthenticationException("Usuário não autenticado.");
            }

            if (identity.Name.Contains("_"))
            {
                return identity.Name.Split('_').Last();
            }

            throw new InvalidCredentialException("Contexto de autenticação corrompido.");
        }

        /// <summary>
        /// Recupera o Id do usuário logado
        /// </summary>
        /// <returns>Id do usuário logado</returns>
        public Guid GetCurrentUserId()
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new AuthenticationException("Usuário não autenticado.");
            }

            Guid guid;
            if (identity.Name.Contains("_") && Guid.TryParse(identity.Name.Split('_')[0], out guid))
            {
                return guid;
            }

            throw new InvalidCredentialException("Contexto de autenticação corrompido.");
        }

        /// <summary>
        /// Marca o usuário como autenticado
        /// </summary>
        /// <param name="userId"> The user Id. </param>
        /// <param name="userName"> The user Name. </param>
        /// <param name="activities"> The activities. </param>
        /// <param name="userProfile"> Perfil do usuário. </param>
        /// <typeparam name="TU"> Tipo de identificador usado para o usuário </typeparam>
        public void SetAuthenticated<TU>(TU userId, string userName, string activities, string userProfile = null)
        {
            const int CookieExpiration = 8;

            var profile = userProfile == null ? "" : userProfile + "_";


            // Usar com [AuthorizeActivity(new[] { ControllerActivityEnum.ActivityName })] na ação do controller.
            var authTicket = new FormsAuthenticationTicket(
                userId + "_" + profile + userName, // user name
                true, // is persistent?
                CookieExpiration * 60); // timeout in minutes

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);

            // Criamos o cookie de atividades
            if (!string.IsNullOrWhiteSpace(activities))
            {
                var cookie = new HttpCookie(
                    ActivityCookieName, SimpleCrypto.EncryptString(activities, ActivityCookieSecret));

                cookie.Expires = DateTime.Now.AddHours(CookieExpiration);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Recupera objetos do repositório do Request
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="key">Chave onde é armazenado o objeto</param>
        /// <returns>Objeto, caso ele esteja no contexto, ou novo objeto</returns>
        public T Get<T>(string key)
        {
            if (this.Items != null)
            {
                return (T)this.Items[key];
            }

            return this.Fallback.Get<T>(key);
        }

        /// <summary>
        /// Persiste objetos no repositório compartilhado do contexto da aplicação
        /// </summary>
        /// <typeparam name="T">O tipo do objeto</typeparam>
        /// <param name="key">A chave do objeto no repositório</param>
        /// <param name="value">O objeto</param>
        public void Set<T>(string key, T value)
        {
            if (this.Items != null)
            {
                this.Items[key] = value;
            }
            else
            {
                this.Fallback.Set(key, value);
            }
        }

        /// <summary>
        /// Verifica se o usuário está autorizado a desempenhar a atividade
        /// </summary>
        /// <param name="activity">ID da atividade</param>
        /// <returns>Verdadeiro caso tenha permissão</returns>
        public bool IsAuthorized(string activity)
        {
            if (HttpContext.Current == null)
            {
                return false;
            }

            var cookie = HttpContext.Current.Request.Cookies[ActivityCookieName];
            if (cookie == null)
            {
                if (this.IsAuthenticated())
                {
                    // Se o usuário está autenticado mas não tem cookie de atividades permitidas, um expirou antes do outro..
                    // Vamos deslogar pra limpar ambos.
                    Logger.Instance.LogEvent("Cookie de atividades expirou antes do cookie de login. Deslogando para sincronizar ambos.");
                    this.LogOff();
                }

                return false;
            }

            try
            {
                var activities = SimpleCrypto.DecryptString(cookie.Value, ActivityCookieSecret);
                var list = Json.Decode<List<string>>(activities);
                return list != null && list.Contains(activity);
            }
            catch (Exception ex)
            {
                // Em caso de erro na decodificação do cookie de atividades permitidas
                // efetuamos logoff para limpar tanto ele quanto o cookie de login
                Logger.Instance.LogException(new InvalidCredentialException("Erro ao decodificar cookie de atividades permitidas. Expiração?", ex));
                this.LogOff();
                return false;
            }
        }

        /// <summary>
        /// Retorna o endereço do usuário atual
        /// </summary>
        /// <returns>O endereço</returns>
        public string GetCurrentAddress()
        {
            var query = HttpContext.Current.Request.QueryString["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(query))
            {
                return query;
            }

            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// Retorna o caminho físico
        /// </summary>
        /// <param name="path">O caminho</param>
        /// <returns>Endereço Físico</returns>
        public string GetMapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path);
        }

        /// <summary>
        /// Response Cookie
        /// </summary>
        /// <param name="cookie">O Cookie</param>
        public void ResponseCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Request Cookie
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>O Cookie</returns>
        public HttpCookie RequestCookie(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// Valor do cookie
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>Valor do Cookie</returns>
        public string RequestCookieValue(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            return cookie == null ? "null" : cookie.Value;
        }

        /// <summary>
        /// Dicionário com os valores dos subcookies
        /// </summary>
        /// <param name="name">nome do cookie</param>
        /// <returns>valores dos subcookies</returns>
        public Dictionary<string, string> RequestCookieDictionary(string name)
        {
            var value = this.RequestCookieValue(name);
            if (value == null)
            {
                return null;
            }

            var result = new Dictionary<string, string>();
            var parts = value.Split('&');
            foreach (var part in parts)
            {
                var point = part.IndexOf('=');
                if (point >= 0)
                {
                    result[part.Substring(0, point)] = (point + 1) < part.Length
                                                           ? part.Substring(point + 1)
                                                           : string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// Efetua logoff
        /// </summary>
        public void LogOff()
        {
            FormsAuthentication.SignOut();

            // Para Sign-out que não funciona: (fonte: http://stackoverflow.com/questions/412300/formsauthentication-signout-does-not-log-the-user-out)
            // clear authentication cookie
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
            authCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            var sessionCookie = new HttpCookie("ASP.NET_SessionId", string.Empty);
            sessionCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(sessionCookie);

            var cookie = HttpContext.Current.Response.Cookies[ActivityCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
            }
        }

        /// <summary>
        /// Recupera parâmetros de navegação (Query Strings, onde fizer sentido)
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <returns>A string do parâmetro</returns>
        public string GetNavigationParameter(string name)
        {
            return HttpContext.Current != null ? HttpContext.Current.Request.QueryString[name] : null;
        }

        /// <summary>
        /// Recupera os cabeçalhos da requisição
        /// </summary>
        /// <param name="header">O cabecalho</param>
        /// <returns>Retorna um cabecalho do request</returns>
        public string GetRequestHeader(string header)
        {
            return HttpContext.Current.Request.Headers[header];
        }
    }
}
