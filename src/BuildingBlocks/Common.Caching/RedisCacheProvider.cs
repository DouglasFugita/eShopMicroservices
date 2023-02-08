using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Common.Caching;
public class RedisCacheProvider : IRedisCacheProvider
{
    private readonly IDistributedCache _cache;

    public RedisCacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key)
    {
        var value = _cache.GetString(key);
        if (value != null)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }

    public void Set<T>(string key, T value)
    {
        var timeout = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
            SlidingExpiration = TimeSpan.FromMinutes(1),
        };

        _cache.SetString(key, JsonSerializer.Serialize(value), timeout);
    }

    public void Remove<T>(string key)
    {
        _cache.Remove(key);
    }
}
