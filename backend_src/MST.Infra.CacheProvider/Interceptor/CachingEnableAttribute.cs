using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using MST.Infra.CacheProvider.KeyGenerator;
using Newtonsoft.Json;
using Rougamo;
using Rougamo.Context;

namespace MST.Infra.CacheProvider.Interceptor;

[AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = false)]
public class CachingEnableAttribute:MoAttribute,IDisposable
{
    private static IServiceProvider _serviceProvider;

    private IServiceScope _scope;
    private ICacheProvider _cacheProvider;
    private readonly IRedisClient _redisClient;
    private readonly ICacheKeyGenerator _cachingKeyGenerator;
    private RedisClient.LockController _lockController;

    // 启动时需要注入根服务器
    public static void SetServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    public CachingEnableAttribute(string customKey = "")
    {
        CacheKey = customKey;
        _scope = _serviceProvider.CreateScope();
        _cacheProvider = _scope.ServiceProvider.GetRequiredService<ICacheProvider>();
        _cacheProvider = _scope.ServiceProvider.GetRequiredService<ICacheProvider>();
        _redisClient = _scope.ServiceProvider.GetRequiredService<IRedisClient>();
        _cachingKeyGenerator = _scope.ServiceProvider.GetRequiredService<ICacheKeyGenerator>();
    }

    public string CacheKey { get; set; }

    public override void OnEntry(MethodContext context)
    {
        
        if (string.IsNullOrWhiteSpace(CacheKey))
            CacheKey = _cachingKeyGenerator.GeneratorKey(context);
        if (_redisClient.Exists(CacheKey))
        {
            var resStr = _redisClient.Get(CacheKey);
            var realResult = string.IsNullOrWhiteSpace(resStr)?null:JsonConvert.DeserializeObject(resStr, context.RealReturnType);
            context.ReplaceReturnValue(this, realResult);
        }
        else
        {
            // 并发量大需要处理缓存击穿问题 用互斥锁   
            // 超时默认10秒，如果没有获取到的话也无所谓 只要不会有大量请求跑数据库就行
            _lockController = _redisClient.Lock("Lock" + CacheKey,10);
            //_redLock.IsAcquired 不需要这句
            // 获取锁后再判断一次，如果已经有了就不用去数据库再读了
            if (_redisClient.Exists(CacheKey))
            {
                var resStr = _redisClient.Get(CacheKey);
                var realResult =string.IsNullOrWhiteSpace(resStr)?null:  JsonConvert.DeserializeObject(resStr, context.RealReturnType);
                context.ReplaceReturnValue(this, realResult);
                _lockController.Dispose();
            }
        }
    }

    public override void OnException(MethodContext context)
    {
        this.Dispose();
    }

    public override void OnSuccess(MethodContext context)
    {
        if (typeof(Task).IsAssignableFrom(context.RealReturnType))
            ((Task)context.ReturnValue).ContinueWith(t => _OnExit());
        else _OnExit();

        void _OnExit()
        {
            try
            {
            }
            finally
            {
                this.Dispose();
            }
        }
    }


    public void Dispose()
    {
        _scope.Dispose();
        _lockController.Dispose();
    }
}