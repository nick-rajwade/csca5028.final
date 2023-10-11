using csca5028.final.project.components;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace csca5028.blazor.webapp.Data
{
    public class RedisCachingService
    {
        private readonly IDistributedCache _cache;

        public RedisCachingService(IDistributedCache redisCache)
        {
            _cache = redisCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value == null ? default(T) : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
        
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();
            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            await _cache.SetStringAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(value), options);
        }
    }
}
