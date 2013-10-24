namespace MTO.Practices.Cache.Couchbase
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;

    using global::Couchbase;

    using Enyim.Caching.Memcached;

    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

    /// <summary>
    /// Implementação padrão para o cache ativo usando CouchBase para cache distribuido e o Enterprise Library para cache local.
    /// </summary>
    public class CouchBaseActiveCache : IActiveCache
    {
        /// <summary>
        /// Cliente do couchbase
        /// </summary>
        private static readonly CouchbaseClient RemoteClient = ActiveCache.UseRemoteCache ? new CouchbaseClient() : null;

        /// <summary>
        /// Dicionário para criar lock no refresh de dados e evitar que ele seja feito simultaneamente em vários requests ao mesmo server
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> KeysRefreshing = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Instância do cache manager
        /// </summary>
        private static ICacheManager localClient;

        /// <summary>
        /// Singleton para a instância do cache
        /// </summary>
        private static ICacheManager LocalClient
        {
            get
            {
                // Pode ocorrer exception nesta linha ao debugar no visual studio. É um erro conhecido do enterprise library 5, não acontece fora do visual studio.
                return localClient ?? (localClient = CacheFactory.GetCacheManager("LocalCache"));
            }
        }

        /// <summary>
        /// Recupera dado do cache local
        /// </summary>
        /// <param name="key">chave do cache buscado</param>
        /// <param name="fallbackCache">idica se deve ser feita busca em cache distribuido caso a chave não seja encontrada no cache local</param>
        /// <param name="refreshMethod">método usado para atualizar o cache, caso ele não exista </param>
        /// <param name="localTtl">Tempo de vida do cache local em segundos </param>
        /// <param name="remoteTtl">Tempo de vida do cache remoto em segundos </param>
        /// <param name="slidingExpiration">Se o tempo de vida deve ser contado desde a última requisição do item no cache</param>
        /// <param name="force">Força uso do cache mesmo quando este está desabilitado</param>
        /// <typeparam name="T">O tipo do objeto retornado do cache</typeparam>
        /// <returns>conteúdo do cache</returns>
        public T GetLocalCache<T>(string key, bool fallbackCache = true, Delegates.RefreshCacheDelegate<T> refreshMethod = null, int? localTtl = null, int? remoteTtl = null, bool slidingExpiration = false, bool force = false) where T : class
        {
            T value = null;

            if (ActiveCache.UseLocalCache || force)
            {
                value = LocalClient.GetData(key) as T;
            }

            if (value == null)
            {
                // se recebemos expiração usamos ela
                if (ActiveCache.UseRemoteCache && fallbackCache)
                {
                    value = this.GetRemote(key, refreshMethod, remoteTtl);

                    this.StoreLocal(key, value, false, localTtl, remoteTtl, slidingExpiration, force);
                }
                else if (refreshMethod != null)
                {
                    value = Refresh(key, refreshMethod, true, localTtl, remoteTtl, slidingExpiration, force);
                }
            }

            return value;
        }

        /// <summary>
        /// Recupera dado do cache remoto
        /// </summary>
        /// <param name="key">chave do cache buscado</param>
        /// <param name="refreshMethod">método usado para atualizar o cache, caso ele não exista </param>
        /// <param name="ttl">Tempo de vida do cache em segundos</param>
        /// <typeparam name="T">O tipo do objeto retornado do cache</typeparam>
        /// <returns>conteúdo do cache</returns>
        public T GetRemote<T>(string key, Delegates.RefreshCacheDelegate<T> refreshMethod = null, int? ttl = null) where T : class
        {
            object value = null;

            // Eu sei que da pra simplificar essa operação lógica, mas deixa assim que fica mais claro:
            var getFailed = ActiveCache.UseRemoteCache ? !RemoteClient.TryGet(key, out value) : true;

            if (getFailed && refreshMethod != null)
            {
                ttl = ttl ?? ActiveCache.CacheTime;

                // Fazemos cache local de mesmo tempo do remoto
                return Refresh(key, refreshMethod, storeLocal: false, localTtl: ttl, remoteTtl: ttl);
            }

            return value as T;
        }

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
        public bool PrependRemote(string key, byte[] value)
        {
            return ActiveCache.UseRemoteCache && RemoteClient.Prepend(key, new ArraySegment<byte>(value));
        }

        /// <summary>
        /// Adiciona um conteúdo a um cache remoto já existente
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
        public bool AppendRemote(string key, byte[] value)
        {
            return ActiveCache.UseRemoteCache && RemoteClient.Append(key, new ArraySegment<byte>(value));
        }

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
        public bool AppendRemote(string key, string value)
        {
            return ActiveCache.UseRemoteCache && RemoteClient.Append(key, new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(value)));
        }

        /// <summary>
        /// Armazena dado em cache local
        /// </summary>
        /// <param name="key">chave do cache</param>
        /// <param name="value">conteúdo do cache</param>
        /// <param name="storeFallback">indica se o conteúdo deve ser armazenado no cache de fallback </param>
        /// <param name="localTtl">tempo de vida do cache local em segundos</param>
        /// <param name="remoteTtl">tempo de vida do cache remoto em segundos</param>
        /// <param name="slidingExpiration">Se a expiração do cache deve ser contada desde a última vez que o valor foi buscado</param>
        /// <param name="force">Força uso do cache mesmo quando este está desabilitado</param>
        /// <typeparam name="T">O tipo do objeto do cache</typeparam>
        public void StoreLocal<T>(string key, T value, bool storeFallback = true, int? localTtl = null, int? remoteTtl = null, bool slidingExpiration = false, bool force = false) where T : class
        {
            if (storeFallback)
            {
                this.StoreRemote(key, value, DateTime.Now.AddSeconds(remoteTtl ?? ActiveCache.CacheTime ?? localTtl ?? ActiveCache.LocalCacheTime));
            }

            if (ActiveCache.UseLocalCache || force)
            {
                var seconds = localTtl ?? ActiveCache.LocalCacheTime;
                var expiration = slidingExpiration ? (ICacheItemExpiration)new SlidingTime(new TimeSpan(0, 0, 0, seconds)) : new AbsoluteTime(DateTime.Now.AddSeconds(seconds));

                LocalClient.Add(
                    key,
                    value,
                    CacheItemPriority.None,
                    null,
                    expiration);
            }
        }

        /// <summary>
        /// Armazena dado em cache distribuido
        /// </summary>
        /// <param name="key">chave do cache</param>
        /// <param name="value">conteúdo do cache</param>
        /// <param name="expireDate">data de expiração do cache</param>
        /// <typeparam name="T">O tipo do objeto do cache</typeparam>
        public void StoreRemote<T>(string key, T value, DateTime? expireDate = null) where T : class
        {
            if (ActiveCache.UseRemoteCache)
            {
                if (expireDate.HasValue)
                {
                    RemoteClient.Store(StoreMode.Set, key, value, expireDate.Value);
                }
                else
                {
                    RemoteClient.Store(StoreMode.Set, key, value);
                }
            }
        }

        /// <summary>
        /// Remove conteúdo de cache local
        /// </summary>
        /// <param name="key">chave a ser removida</param>
        /// <param name="removeFallback">indica se o conteúdo também deve ser removido do cache de fallback</param>
        public void RemoveLocal(string key, bool removeFallback = true)
        {
            // não precisa checar se UseLocal..
            LocalClient.Remove(key);

            if (removeFallback)
            {
                this.RemoveRemote(key);
            }
        }

        /// <summary>
        /// Remove conteúdo de cache remoto
        /// </summary>
        /// <param name="key">chave a ser removida</param>
        public void RemoveRemote(string key)
        {
            if (ActiveCache.UseRemoteCache)
            {
                RemoteClient.Remove(key);
            }
        }

        /// <summary>
        /// Conta os itens armazenados no cache local
        /// </summary>
        /// <returns>A quantidade de itens</returns>
        public int CountLocal()
        {
            return LocalClient.Count;
        }

        /// <summary>
        /// Conta os itens armazenados em cada servidor remoto
        /// </summary>
        /// <returns>A quantidade de itens por sevidor</returns>
        public NameValueCollection CountRemote()
        {
            if (RemoteClient == null)
            {
                return null;
            }

            var serverResponse = RemoteClient.Stats();
            var stats = serverResponse.GetRaw("curr_items");
            var result = new NameValueCollection();

            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    result.Add(stat.Key.Address + ":" + stat.Key.Port, stat.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Recupera o serial do valor no cache remoto
        /// </summary>
        /// <param name="key">A chave</param>
        /// <returns>O serial</returns>
        public ulong GetRemoteCas(string key)
        {
            return RemoteClient.GetWithCas(key).Cas;
        }

        /// <summary>
        /// Calcula o valor que deve ser utilizado na atualização do cache
        /// </summary>
        /// <param name="key">chave a ser atualizada</param>
        /// <param name="refreshMethod">delegate a ser invocado</param>
        /// <param name="storeLocal">indica se o cache primário é local ou remoto</param>
        /// <param name="localTtl">tempo de vida do cache local em segundos</param>
        /// <param name="remoteTtl">tempo de vida do cache remoto em segundos</param>
        /// <param name="slidingExpiration"> se o tempo de expiração é calculado a partir da ultima vez que o objeto foi recuperado do cache</param>
        /// <param name="force">Força uso do cache mesmo quando este está desabilitado</param>
        /// <typeparam name="T">O tipo do objeto do cache</typeparam>
        /// <returns>conteúdo a ser armazenado em cache</returns>
        private static T Refresh<T>(string key, Delegates.RefreshCacheDelegate<T> refreshMethod, bool storeLocal = false, int? localTtl = null, int? remoteTtl = null, bool slidingExpiration = false, bool force = false) where T : class
        {
            var locker = KeysRefreshing.GetOrAdd(key, s => new object());
            lock (locker)
            {
                T value = null;

                // Verifica se o valor foi armazenado em cache após o lock
                if (storeLocal)
                {
                    if (ActiveCache.UseLocalCache || force)
                    {
                        value = LocalClient.GetData(key) as T;
                    }
                }
                else
                {
                    object tryget;
                    if (ActiveCache.UseRemoteCache && RemoteClient.TryGet(key, out tryget))
                    {
                        value = tryget as T;
                    }
                }

                // Valor foi armazenado no cache após o lock.
                if (value != null)
                {
                    return value;
                }

                // invoca refresh
                value = refreshMethod();

                // armazena dado em cache
                if (storeLocal)
                {
                    if (ActiveCache.UseLocalCache || force)
                    {
                        localTtl = localTtl ?? ActiveCache.LocalCacheTime;
                        var expiration = slidingExpiration
                                             ? (ICacheItemExpiration)new SlidingTime(new TimeSpan(0, 0, 0, localTtl.Value))
                                             : new AbsoluteTime(DateTime.Now.AddSeconds(localTtl.Value));

                        LocalClient.Add(key, value, CacheItemPriority.None, null, expiration);
                    }
                }
                else if (ActiveCache.UseRemoteCache)
                {
                    if (remoteTtl.HasValue)
                    {
                        RemoteClient.Store(StoreMode.Set, key, value, DateTime.Now.AddSeconds(remoteTtl.Value));
                    }
                    else
                    {
                        RemoteClient.Store(StoreMode.Set, key, value);
                    }
                }

                // Remove item do dicionário de locks
                object removed;
                KeysRefreshing.TryRemove(key, out removed);

                return value;
            }
        }
    }
}
