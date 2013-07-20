namespace MTO.Practices.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Web;

    using MTO.Practices.Common.Interfaces;

    /// <summary>
    /// Contrato para repositórios de contexto
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Constroi nova instância do contexto
        /// </summary>
        /// <returns>nova instância do contexto</returns>
        IContext NewInstance();

        /// <summary>
        /// Credenciais da thread atual
        /// </summary>
        ConcurrentDictionary<string, ICredential> Credentials { get; set; }

        /// <summary>
        /// Recupera se o usuário está autenticado
        /// </summary>
        /// <returns>Verdadeiro se o usuário está autenticado</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Recupera o perfil do usuário logado
        /// </summary>
        /// <returns>Perfil do usuário logado</returns>
        string GetUserProfile();

        /// <summary>
        /// Recupera o nome do usuário logado
        /// </summary>
        /// <returns>Nome do usuário logado</returns>
        string GetCurrentUserName();

        /// <summary>
        /// Recupera o Id do usuário logado
        /// </summary>
        /// <returns>Id do usuário logado</returns>
        Guid GetCurrentUserId();

        /// <summary>
        /// Marca o usuário como autenticado
        /// </summary>
        /// <param name="userId"> The user Id. </param>
        /// <param name="userName"> The user Name. </param>
        /// <param name="activities"> The activities. </param>
        /// <param name="userProfile"> Perfil do usuário. </param>
        /// <typeparam name="U"> Tipo de identificador usado para o usuário </typeparam>
        void SetAuthenticated<U>(U userId, string userName, string activities, string userProfile = null);

        /// <summary>
        /// Recupera objetos do repositório compartilhado do contexto da aplicação
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="key">A chave do objeto no repositório</param>
        /// <returns>O objeto</returns>
        T Get<T>(string key);

        /// <summary>
        /// Persiste objetos no repositório compartilhado do contexto da aplicação
        /// </summary>
        /// <typeparam name="T">O tipo do objeto</typeparam>
        /// <param name="key">A chave do objeto no repositório</param>
        /// <param name="value">O objeto</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Verifica se o usuário está autorizado a desempenhar a atividade
        /// </summary>
        /// <param name="activity">ID da atividade</param>
        /// <returns>Verdadeiro caso tenha permissão</returns>
        bool IsAuthorized(string activity);

        /// <summary>
        /// Retorna o endereço do usuário atual
        /// </summary>
        /// <returns>O endereço</returns>
        string GetCurrentAddress();

        /// <summary>
        /// Retorna o caminho físico
        /// </summary>
        /// <param name="path">o caminho</param>
        /// <returns>Endereço Físico</returns>
        string GetMapPath(string path);

        /// <summary>
        /// Response Cookie
        /// </summary>
        /// <param name="cookie">O Cookie</param>
        void ResponseCookie(HttpCookie cookie);

        /// <summary>
        /// Recupera cookie do request
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>O Cookie</returns>
        HttpCookie RequestCookie(string name);

        /// <summary>
        /// Valor do cookie
        /// </summary>
        /// <param name="name">Nome do Cookie</param>
        /// <returns>Valor do Cookie</returns>
        string RequestCookieValue(string name);

        /// <summary>
        /// Dicionário com os valores dos subcookies
        /// </summary>
        /// <param name="name">nome do cookie</param>
        /// <returns>valores dos subcookies</returns>
        Dictionary<string, string> RequestCookieDictionary(string name);

        /// <summary>
        /// Efetua logoff
        /// </summary>
        void LogOff();

        /// <summary>
        /// Recupera parâmetros de navegação (Query Strings, onde fizer sentido)
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <returns>A string do parâmetro</returns>
        string GetNavigationParameter(string name);

        /// <summary>
        /// Recupera os cabeçalhos da requisição
        /// </summary>
        /// <param name="header">O cabecalho</param>
        /// <returns>Retorna um cabecalho do request</returns>
        string GetRequestHeader(string header);
    }
}
