namespace Common.Caching;
public interface IRedisCacheProvider
{
    T? Get<T>(string key);
    void Set<T>(string key, T value);
    void Remove<T>(string key);
}
