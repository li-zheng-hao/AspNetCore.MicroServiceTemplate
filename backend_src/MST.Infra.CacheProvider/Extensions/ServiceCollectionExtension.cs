using FreeRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MST.Infra.CacheProvider.Interceptor;
using MST.Infra.CacheProvider.KeyGenerator;
using MST.Infra.Configuration;
using Quickwire;

namespace MST.Infra.CacheProvider.Extensions;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddRedisCaching(this IServiceCollection services)
    {
        services.ScanCurrentAssembly();
        services.TryAddSingleton<IRedisClient>(sp =>
        {
            var options = sp.GetRequiredService<RedisOptions>();
            return new RedisClient(options.ConnectionString);
        });
        services.TryAddSingleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>();
        return services;
    }
    public static WebApplication UseRedisCaching(this WebApplication app)
    {
        app.Use(async (httpContext, next) =>
        {
            CachingEnableAttribute.SetServiceProvider(httpContext.RequestServices);
            ClearCacheAttribute.SetServiceProvider(httpContext.RequestServices);
            await next();
        });
        return app;
    }
}