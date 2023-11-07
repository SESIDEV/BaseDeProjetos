using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

public class DbCache
{
    private readonly IDistributedCache _cache;
    private const string CacheKeyListKey = "CacheKeyList";

    // Semaphore para threading
    private readonly SemaphoreSlim _cacheKeyListSemaphore = new SemaphoreSlim(1, 1);


    public DbCache(IDistributedCache cache)
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
            var keys = await GetCacheKeyListAsync();
            keys.Add(cacheKey);
            await _cache.SetStringAsync(CacheKeyListKey, JsonConvert.SerializeObject(keys));
        }
        finally
        {
            _cacheKeyListSemaphore.Release();
        }
    }

    /// <summary>
    /// Obtém a lista de keys
    /// </summary>
    /// <returns></returns>
    private async Task<HashSet<string>> GetCacheKeyListAsync()
    {
        var keysJson = await _cache.GetStringAsync(CacheKeyListKey);
        return keysJson != null ? JsonConvert.DeserializeObject<HashSet<string>>(keysJson) : new HashSet<string>();
    }

    /// <summary>
    /// Obtem/Registra os dados no cache (não async). Os dados são invalidados após 1h de forma automática caso não especificado
    /// </summary>
    /// <param name="cacheKey">Chave dos dados a serem obtidos ou registrados para cache</param>
    /// <param name="retrieveDataFunc">Método de obtenção dos dados</param>
    /// <param name="expiration">Opcional: tempo em horas de expiração do cache</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetCachedAsync<T>(string cacheKey, Func<T> retrieveDataFunc, TimeSpan? expiration = null)
    {
        string serializedData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(serializedData))
        {
            return JsonConvert.DeserializeObject<T>(serializedData);
        }

        T data = retrieveDataFunc();

        if (data != null)
        {
            Console.WriteLine($"Cache miss or null data for: {cacheKey}");
            var options = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1) };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data, options), cacheOptions);
        }

        return data;
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
        string serializedData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(serializedData))
        {
            return JsonConvert.DeserializeObject<T>(serializedData);
        }

        T data = await retrieveDataFunc();

        if (data != null)
        {
            var options = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1) };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data, options), cacheOptions);
        }
        else
        {
            Console.WriteLine($"No cache data for {cacheKey}");
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
    public async Task SetCachedAsync<T>(string cacheKey, T value, TimeSpan? duration = null)
    {
        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = duration ?? TimeSpan.FromHours(1)
        };

        var options = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        var serializedValue = JsonConvert.SerializeObject(value, options);
        await _cache.SetStringAsync(cacheKey, serializedValue, cacheEntryOptions);
    }


    /// <summary>
    /// Invalida dados do cache de acordo com a chave na qual ele foi registrado
    /// </summary>
    /// <param name="cacheKey"></param>
    public async Task InvalidateCacheAsync(string cacheKey)
    {
        await _cache.RemoveAsync(cacheKey);
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
            var keys = await GetCacheKeyListAsync();
            var keysToRemove = new HashSet<string>();

            foreach (var key in keys)
            {
                if (key.Contains(keyContent))
                {
                    await _cache.RemoveAsync(key);
                }
            }

            await _cache.RemoveAsync(CacheKeyListKey);

            keys.ExceptWith(keysToRemove);

            if (keys.Count > 0)
            {
                await _cache.SetStringAsync(CacheKeyListKey, JsonConvert.SerializeObject(keys));
            }
            else
            {
                await _cache.RemoveAsync(CacheKeyListKey);
            }
        }finally {
            _cacheKeyListSemaphore.Release();
        }
    }
}