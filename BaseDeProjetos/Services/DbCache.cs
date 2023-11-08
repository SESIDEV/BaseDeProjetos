using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

public class DbCache
{
    private readonly IMemoryCache _cache;
    private const string CacheKeyListKey = "CacheKeyList";

    // Semaphore for thread safety when accessing the cache key list
    private readonly SemaphoreSlim _cacheKeyListSemaphore = new SemaphoreSlim(1, 1);

    public DbCache(IMemoryCache cache)
    {
        _cache = cache;
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

    private HashSet<string> GetCacheKeyList()
    {
        return _cache.GetOrCreate(CacheKeyListKey, entry => new HashSet<string>());
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
            }
            else
            {
                Console.WriteLine($"No data in cache for {cacheKey}");
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
            else
            {
                Console.WriteLine($"No data in cache for {cacheKey}");
            }
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
