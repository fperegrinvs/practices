namespace MTO.Practices.Cache
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Interface de cache ativo
    /// </summary>
    public interface IActiveCache
    {
        /// <summary>
        /// Recupera dado do cache local
        /// </summary>
        /// <param name="key">chave do cache buscado</param>
        /// <param name="fallbackCache">idica se deve ser feita busca em cache distribuido caso a chave não seja encontrada no cache local</param>
        /// <param name="refreshMethod">método usado para atualizar o cache, caso ele não exista </param>
        /// <param name="localTtl">Tempo de vida do cache local em segundos </param>
        /// <param name="remoteTtl">Tempo de vida do cache remoto em segundos </param>
        /// <param name="slidingExpiration">Se o tempo de vida deve ser contado desde a última requisição do item no cache</param>
        /// <typeparam name="T">O tipo do objeto retornado do cache</typeparam>
        /// <returns>conteúdo do cache</returns>
        T GetLocalCache<T>(string key, bool fallbackCache = true, Delegates.RefreshCacheDelegate<T> refreshMethod = null, int? localTtl = null, int? remoteTtl = null, bool slidingExpiration = false) where T : class;

        /// <summary>
        /// Recupera dado do cache remoto
        /// </summary>
        /// <param name="key">chave do cache buscado</param>
        /// <param name="refreshMethod">método usado para atualizar o cache, caso ele não exista </param>
        /// <param name="ttl"> tempo de vida do cache em segundos</param>
        /// <typeparam name="T">O tipo do objeto retornado do cache</typeparam>
        /// <returns>conteúdo do cache</returns>
        T GetRemote<T>(string key, Delegates.RefreshCacheDelegate<T> refreshMethod = null, int? ttl = null) where T : class;

        /// <summary>
        /// Adiciona um conteúdo ao inicio de um cache remoto já existente
        /// </summary>
        /// <param name="key">
        /// chave do cache
        /// </param>
        /// <param name="value">
        /// valor a ser armazenado em cache
        /// </param>
        /// <returns>
        /// Indica se a operação foi bem sucedida ou não
        /// </returns>
        bool PrependRemote(string key, byte[] value);

        /// <summary>
        /// Adiciona um conteúdo ao fim de um cache remoto já existente
        /// </summary>
        /// <param name="key">
        /// chave do cache
        /// </param>
        /// <param name="value">
        /// valor a ser armazenado em cache
        /// </param>
        /// <returns>
        /// Indica se a operação foi bem sucedida ou não
        /// </returns>
        bool AppendRemote(string key, byte[] value);

        /// <summary>
        /// Adiciona um conteúdo ao fim de um cache remoto já existente
        /// </summary>
        /// <param name="key">
        /// chave do cache
        /// </param>
        /// <param name="value">
        /// valor a ser armazenado em cache
        /// </param>
        /// <returns>
        /// Indica se a operação foi bem sucedida ou não
        /// </returns>
        bool AppendRemote(string key, string value);

        /// <summary>
        /// Armazena dado em cache local
        /// </summary>
        /// <param name="key">chave do cache</param>
        /// <param name="value">conteúdo do cache</param>
        /// <param name="storeFallback">indica se o conteúdo deve ser armazenado no cache de fallback </param>
        /// <param name="localTtl">tempo de vida do cache local em segundos</param>
        /// <param name="remoteTtl">tempo de vida do cache remoto em segundos</param>
        /// <param name="slidingExpiration">Se a expiração do cache deve ser contada desde a última vez que o valor foi buscado</param>
        /// <typeparam name="T">O tipo do objeto do cache</typeparam>
        void StoreLocal<T>(string key, T value, bool storeFallback = true, int? localTtl = null, int? remoteTtl = null, bool slidingExpiration = false) where T : class;

        /// <summary>
        /// Armazena dado em cache distribuido
        /// </summary>
        /// <param name="key">chave do cache</param>
        /// <param name="value">conteúdo do cache</param>
        /// <param name="expireDate">data de expiração do cache</param>
        /// <typeparam name="T">O tipo do objeto do cache</typeparam>
        void StoreRemote<T>(string key, T value, DateTime? expireDate = null) where T : class;

        /// <summary>
        /// Remove conteúdo de cache local
        /// </summary>
        /// <param name="key">chave a ser removida</param>
        /// <param name="removeFallback">indica se o conteúdo também deve ser removido do cache de fallback</param>
        void RemoveLocal(string key, bool removeFallback = true);

        /// <summary>
        /// Remove conteúdo de cache remoto
        /// </summary>
        /// <param name="key">chave a ser removida</param>
        void RemoveRemote(string key);

        /// <summary>
        /// Conta os itens armazenados no cache local
        /// </summary>
        /// <returns>A quantidade de itens</returns>
        int CountLocal();

        /// <summary>
        /// Conta os itens armazenados em cada servidor remoto
        /// </summary>
        /// <returns>A quantidade de itens por sevidor</returns>
        NameValueCollection CountRemote();

        /// <summary>
        /// Recupera o serial do valor no cache remoto
        /// </summary>
        /// <param name="key">A chave</param>
        /// <returns>O serial</returns>
        ulong GetRemoteCas(string key);
    }
}
