using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevShop.Core.Datas.Interfaces;

public interface IRedisRepository
{
    Task<TCache> GetCache<TCache>(string key);
    Task<TCache> GetCacheByte<TCache>(string key);
    Task<List<TCache>> GetCacheList<TCache>(string key);
    Task SetCache<TCache>(string key, TCache value, double timeExpiration = -1);
    Task SetCacheByte<TCache>(string key, TCache value, double timeExpiration = -1);
    Task SetCache<TCache>(string key, TCache value, DistributedCacheEntryOptions options);
    Task RemoveCache(string key);
    Task UpdateCache(string key);
}