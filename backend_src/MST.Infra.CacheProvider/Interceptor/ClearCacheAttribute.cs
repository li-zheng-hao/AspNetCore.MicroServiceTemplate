using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using MST.Infra.CacheProvider.KeyGenerator;
using Rougamo;
using Rougamo.Context;

namespace MST.Infra.CacheProvider.Interceptor;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ClearCacheAttribute : Rougamo.MoAttribute
{
    #region 需要的属性

    public bool IsBefore { get; set; }
    public string KeyPrefix { get; set; }

    #endregion


    private static IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;
    private readonly ICacheKeyGenerator _cacheKeyGenerator;
    public IRedisClient _redisClient { get; set; }
    public static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;


    /// <summary>
    /// 清除对应key前缀的缓存
    /// </summary>
    /// <param name="keyPrefix"></param>
    public ClearCacheAttribute(string keyPrefix, bool isBefore = false)
    {
        IsBefore = isBefore;
        KeyPrefix = keyPrefix;
        _scope = _serviceProvider.CreateScope();
        _redisClient = _scope.ServiceProvider.GetRequiredService<IRedisClient>();
        _cacheKeyGenerator = _scope.ServiceProvider.GetRequiredService<ICacheKeyGenerator>();
    }
    /// <summary>
    /// 清除对应key前缀的缓存
    /// </summary>
    /// <param name="keyPrefix"></param>
    public ClearCacheAttribute(Type type,string methodeName, bool isBefore = false)
    {
        IsBefore = isBefore;
        KeyPrefix = $"{type.Namespace}:{methodeName}";
        _scope = _serviceProvider.CreateScope();
        _redisClient = _scope.ServiceProvider.GetRequiredService<IRedisClient>();
        _cacheKeyGenerator = _scope.ServiceProvider.GetRequiredService<ICacheKeyGenerator>();
    }

    public override void OnEntry(MethodContext context)
    {
        // 换成在方法执行完后删除
        if (IsBefore)
            ClearCaching();
    }

    public override void OnExit(MethodContext context)
    {
        _scope.Dispose();    
    }
    public override void OnSuccess(MethodContext context)
    {
        if (!IsBefore)
            ClearCaching();
    }

    private void ClearCaching()
    {
        foreach (var keys in _redisClient.Scan($"{KeyPrefix}*", int.MaxValue, null))
            _redisClient.UnLink(keys);
    }
}