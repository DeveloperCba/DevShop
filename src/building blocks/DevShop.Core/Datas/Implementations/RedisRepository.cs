using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevShop.Core.Comunications;
using DevShop.Core.Datas.Interfaces;

namespace DevShop.Core.Datas.Implementations;

public class RedisRepository : IRedisRepository
{
    private readonly IDistributedCache _cache;
    private readonly RedisSettings _cacheSettings;


    public RedisRepository(
        IDistributedCache cache,
        IOptions<RedisSettings> cacheSettings)
    {
        _cache = cache;
        _cacheSettings = cacheSettings.Value;
    }

    public async Task<TCache> GetCache<TCache>(string key)
    {
        var cacheResponse = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cacheResponse))
            return JsonConvert.DeserializeObject<TCache>(cacheResponse);

        return default;
    }

    public async Task<TCache> GetCacheByte<TCache>(string key)
    {
        var cacheResponse = await _cache.GetAsync(key);
        if (cacheResponse != null && cacheResponse.Length > 0)
            return JsonConvert.DeserializeObject<TCache>(Encoding.UTF8.GetString(cacheResponse));

        return default;
    }

    public async Task<List<TCache>> GetCacheList<TCache>(string key)
    {
        var cacheResponse = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cacheResponse)) return null;

        return JsonConvert.DeserializeObject<List<TCache>>(cacheResponse);

    }

    public async Task SetCache<TCache>(string key, TCache value, double timeExpiration = -1)
    {
        var response = JsonConvert.SerializeObject(value);

        if (timeExpiration == -1)
            await _cache.SetStringAsync(key, response, GetOptionsCache());
        else
            await _cache.SetStringAsync(key, response, GetOptionsCache(timeExpiration));
    }

    public async Task SetCacheByte<TCache>(string key, TCache value, double timeExpiration = -1)
    {
        var response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

        if (timeExpiration == -1)
            await _cache.SetAsync(key, response, GetOptionsCache());
        else
            await _cache.SetAsync(key, response, GetOptionsCache(timeExpiration));
    }

    public async Task SetCache<TCache>(string key, TCache value, DistributedCacheEntryOptions options)
    {
        var response = JsonConvert.SerializeObject(value);
        await _cache.SetStringAsync(key, response, options);
    }

    public async Task UpdateCache(string key)
    {
        await _cache.RefreshAsync(key);
    }

    public async Task RemoveCache(string key)
    {
        await _cache.RemoveAsync(key);
    }

    private DistributedCacheEntryOptions GetOptionsCache()
    {
        return new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(_cacheSettings.AbsoluteExpirationInMinutes));
        //.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes))

    }

    private DistributedCacheEntryOptions GetOptionsCache(double minuutes)
    {
        return new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(minuutes));
        //.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.SlidingExpirationInMinutes))

    }
}