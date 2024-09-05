using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class DbCache
{
    // Observações em relaçao a esse cache:
    /*
    1. Esse cache não é tão robusto e se utiliza de um HashSet para armazenar as chaves dos dados armazenados.
    2. A invalidação dessas chaves é crucial para evitar estado "stale" na aplicação, caso isso não seja realizado os dados visíveis estarão desatualizados para o usuário.
    
    3. Esse cache interfere com o lazyloading do EntityFramework nesta versão do .NET Core MVC, ou seja, ao puxar certos dados a aplicação necessitará de um Include.
    No caso, da configuração específica desse projeto, ao remover esse cache o lazyloading funcionou corretamente sem a necessidade de .Include().IncludeFor() etc etc.

    4. O cache é feito de forma assíncrona e síncrona, sendo que a forma assíncrona é mais recomendada para evitar bloqueios de threads.
    Porém em alguns casos pode ser necessário utilizar a forma síncrona.    
    */

    private readonly IMemoryCache _cache;
    private const string CacheKeyListKey = "CacheKeyList";

    // Semaphore for thread safety when accessing the cache key list
    private readonly SemaphoreSlim _cacheKeyListSemaphore = new SemaphoreSlim(1, 1);

    public DbCache(IMemoryCache cache)
    {
        _cache = cache;

        _cache.GetOrCreate(CacheKeyListKey, entry => new HashSet<string>());
    }

    /// <summary>
    /// Adiciona uma chave a lista de chvaes
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    public async Task AddKeyToListAsync(string cacheKey)
    {
        await _cacheKeyListSemaphore.WaitAsync();
        try
        {
            var keys = GetCacheKeyList();
            keys.Add(cacheKey);
            _cache.Set(CacheKeyListKey, keys);
        }
        finally
        {
            _cacheKeyListSemaphore.Release();
        }
    }

    public void AddKeyToList(string cacheKey)
    {
        _cacheKeyListSemaphore.Wait();
        try
        {
            var keys = GetCacheKeyList();
            keys.Add(cacheKey);
            _cache.Set(CacheKeyListKey, keys);
        }
        finally
        {
            _cacheKeyListSemaphore.Release();
        }
    }

    private HashSet<string> GetCacheKeyList()
    {
        // Try to get the existing HashSet from the cache
        if (!_cache.TryGetValue(CacheKeyListKey, out HashSet<string> keys))
        {
            // If it doesn't exist, create a new HashSet, add it to the cache, and return it
            keys = new HashSet<string>();
            _cache.Set(CacheKeyListKey, keys);
        }

        return keys;
    }

    /// <summary>
    /// Obtem/Registra os dados no cache. Os dados são invalidados após 1h de forma automática caso não especificado
    /// </summary>
    /// <param name="cacheKey">Chave dos dados a serem obtidos ou registrados para cache</param>
    /// <param name="retrieveDataFunc">Método de obtenção dos dados</param>
    /// <param name="expiration">Opcional: tempo em horas de expiração do cache</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetCachedAsync<T>(string cacheKey, Func<Task<T>> retrieveDataFunc, TimeSpan? expiration = null)
    {
        if (!_cache.TryGetValue(cacheKey, out T data))
        {
            data = await retrieveDataFunc();
            if (data != null)
            {
                _cache.Set(cacheKey, data, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
                });
                await AddKeyToListAsync(cacheKey);
            }
        }

        return data;
    }

    /// <summary>
    /// Obtem/Registra os dados no cache (não async). Os dados são invalidados após 1h de forma automática caso não especificado
    /// </summary>
    /// <param name="cacheKey">Chave dos dados a serem obtidos ou registrados para cache</param>
    /// <param name="retrieveDataFunc">Método de obtenção dos dados</param>
    /// <param name="expiration">Opcional: tempo em horas de expiração do cache</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCached<T>(string cacheKey, Func<T> retrieveDataFunc, TimeSpan? expiration = null)
    {
        if (!_cache.TryGetValue(cacheKey, out T data))
        {
            data = retrieveDataFunc();
            if (data != null)
            {
                _cache.Set(cacheKey, data, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
                });
            }
            AddKeyToList(cacheKey);
        }

        return data;
    }

    /// <summary>
    /// Guarda os dados em cache de acordo com uma chave e duração padrão de 1h caso não especificada
    /// </summary>
    /// <param name="cacheKey">Chave para Armazenamento dos dados em Cache</param>
    /// <param name="value">Dados</param>
    /// <param name="duration">Duração da Invalidação Automátioca</param>
    /// <typeparam name="T">Tipo Genérico para o Dado</typeparam>
    /// <returns></returns>
    public void SetCached<T>(string cacheKey, T value, TimeSpan? duration = null)
    {
        _cache.Set(cacheKey, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = duration ?? TimeSpan.FromHours(1)
        });

        AddKeyToList(cacheKey);
    }

    /// <summary>
    /// Invalida dados do cache de acordo com a chave na qual ele foi registrado
    /// </summary>
    /// <param name="cacheKey"></param>
    public void InvalidateCache(string cacheKey)
    {
        _cache.Remove(cacheKey);
    }

    /// <summary>
    /// Invalida dados do cache nas keys que contém uma string específica
    /// </summary>
    /// <returns></returns>
    public async Task InvalidateCacheKeysAsync(string keyContent)
    {
        await _cacheKeyListSemaphore.WaitAsync();
        try
        {
            var keys = GetCacheKeyList();
            var keysToRemove = new HashSet<string>();

            foreach (var key in keys)
            {
                if (key.Contains(keyContent))
                {
                    _cache.Remove(key);
                    keysToRemove.Add(key);
                }
            }

            keys.ExceptWith(keysToRemove);
            if (keys.Count > 0)
            {
                _cache.Set(CacheKeyListKey, keys);
            }
            else
            {
                _cache.Remove(CacheKeyListKey);
            }
        }
        finally
        {
            _cacheKeyListSemaphore.Release();
        }
    }
}