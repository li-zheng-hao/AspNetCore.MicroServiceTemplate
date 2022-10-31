using Microsoft.Extensions.Options;

namespace MST.Infra.CacheProvider;

public class FreeRedisCacheProvider:ICacheProvider
{
    public string Name { get; } = nameof(FreeRedisCacheProvider);
    public IOptions<CacheOptions> CacheOptions { get; }
    public void Set<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public T Get<T>(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync<T>(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public bool Exists(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public bool TrySet<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TrySetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public void SetAll<T>(IDictionary<string, T> value, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public void RemoveAll(IEnumerable<string> cacheKeys)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAllAsync(IEnumerable<string> cacheKeys)
    {
        throw new NotImplementedException();
    }

    public T Get<T>(string cacheKey, Func<T> dataRetriever, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration)
    {
        throw new NotImplementedException();
    }

    public void RemoveByPrefix(string prefix)
    {
        throw new NotImplementedException();
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        throw new NotImplementedException();
    }

    public Task<object> GetAsync(string cacheKey, Type type)
    {
        throw new NotImplementedException();
    }

    public Task KeyExpireAsync(IEnumerable<string> cacheKeys, int seconds)
    {
        throw new NotImplementedException();
    }
}