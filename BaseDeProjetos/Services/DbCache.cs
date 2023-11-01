using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

public class DbCache
{
    private readonly IDistributedCache _cache;

    public DbCache(IDistributedCache cache)
    {
        _cache = cache;
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
            return JsonSerializer.Deserialize<T>(serializedData);
        }

        T data = await retrieveDataFunc();

        if (data != null)
        {
            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1) };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);
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

        var serializedValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(cacheKey, serializedValue, cacheEntryOptions);
    }


    /// <summary>
    /// Invalida dados do cache de acordo com a chave na qual ele foi registrado
    /// </summary>
    /// <param name="cacheKey"></param>
    public async Task InvalidateCache(string cacheKey)
    {
        await _cache.RemoveAsync(cacheKey);
    }
}