using System.Text.Json;
using AITrainersHub.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace AITrainersHub.Infrastructure.Implementation
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cachedData = await distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            var serializedData = JsonSerializer.Serialize(value);
            await distributedCache.SetStringAsync(key, serializedData, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}
