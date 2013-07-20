namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;
    using System.Web;

    using MTO.Practices.Common.Interfaces;

    /// <summary>
    /// Repositório de objetos do contexto da Thread
    /// </summary>
    public class ThreadContext : IContext
    {
        /// <summary>
        /// Constroi nova instância do contexto
        /// </summary>
        /// <returns>nova instância do contexto</returns>
        public IContext NewInstance()
        {
            return new ThreadContext();
        }

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
        /// Recupera se o usuário está autenticado
        /// </summary>
        /// <returns>Verdadeiro se o usuário está autenticado</returns>
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(this.Get<string>("UserName"));
        }

        /// <summary>
        /// Recupera o perfil do usuário logado
        /// </summary>
        /// <returns>Perfil do usuário logado</returns>
        public string GetUserProfile()
        {
            return this.Get<string>("UserProfile");
        }

        /// <summary>
        /// Recupera o nome do usuário logado
        /// </summary>
        /// <returns>Nome do usuário logado</returns>
        public string GetCurrentUserName()
        {
            return this.Get<string>("UserName");
        }

        /// <summary>
        /// Recupera o Id do usuário logado
        /// </summary>
        /// <returns>Id do usuário logado</returns>
        public Guid GetCurrentUserId()
        {
            return this.Get<Guid>("UserId");
        }

        /// <summary>
        /// Marca o usuário como autenticado
        /// </summary>
        /// <param name="userId"> The user Id. </param>
        /// <param name="userName"> The user Name. </param>
        /// <param name="activities"> The activities. </param>
        /// <param name="userProfile"> Perfil do usuário. </param>
        /// <typeparam name="U"> Tipo de identificador usado para o usuário </typeparam>
        public void SetAuthenticated<U>(U userId, string userName, string activities, string userProfile = null)
        {
            this.Set("UserId", userId);
            this.Set("UserName", userName);
            this.Set("Activities", activities);
            this.Set("UserProfile", userProfile);
        }

        /// <summary>
        /// Recupera objetos do repositório da Thread
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="key">Chave onde é armazenado o objeto</param>
        /// <returns>Objeto, caso ele esteja no contexto, ou novo objeto</returns>
        public T Get<T>(string key)
        {
            var obj = CallContext.GetData(key);

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Persiste objetos no repositório compartilhado do contexto da aplicação
        /// </summary>
        /// <typeparam name="T">O tipo do objeto</typeparam>
        /// <param name="key">A chave do objeto no repositório</param>
        /// <param name="value">O objeto</param>
        public void Set<T>(string key, T value)
        {
            CallContext.SetData(key, value);
        }

        /// <summary>
        /// Verifica se o usuário está autorizado a desempenhar a atividade
        /// </summary>
        /// <param name="activity">ID da atividade</param>
        /// <returns>Verdadeiro caso tenha permissão</returns>
        public bool IsAuthorized(string activity)
        {
            return true;
        }

        /// <summary>
        /// Retorna o endereço do usuário atual
        /// </summary>
        /// <returns>O endereço</returns>
        public string GetCurrentAddress()
        {
            return "127.0.0.1";
        }

        /// <summary>
        /// Retorna o caminho físico
        /// </summary>
        /// <param name="path">o caminho</param>
        /// <returns>Endereço Físico</returns>
        public string GetMapPath(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Response Cookie
        /// </summary>
        /// <param name="cookie">O Cookie</param>
        public void ResponseCookie(HttpCookie cookie)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recupera cookie do request
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>O Cookie</returns>
        public HttpCookie RequestCookie(string name)
        {
            return null;
        }

        /// <summary>
        /// Valor do cookie
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>Valor do Cookie</returns>
        public string RequestCookieValue(string name)
        {
            return null;
        }

        /// <summary>
        /// Dicionário com os valores dos subcookies
        /// </summary>
        /// <param name="name">nome do cookie</param>
        /// <returns>valores dos subcookies</returns>
        public Dictionary<string, string> RequestCookieDictionary(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Efetua logoff
        /// </summary>
        public void LogOff()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recupera parâmetros de navegação (Query Strings, onde fizer sentido)
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <returns>A string do parâmetro</returns>
        public string GetNavigationParameter(string name)
        {
            return null;
        }

        /// <summary>
        /// Recupera os cabeçalhos da requisição
        /// </summary>
        /// <param name="header">O cabecalho</param>
        /// <returns>Retorna um cabecalho do request</returns>
        public string GetRequestHeader(string header)
        {
            return null;
        }
    }
}
