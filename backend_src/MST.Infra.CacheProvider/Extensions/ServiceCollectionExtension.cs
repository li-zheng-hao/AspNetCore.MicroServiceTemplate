using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MST.Infra.CacheProvider;
using MST.Infra.CacheProvider.KeyGenerator;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMSTInfraRedisCaching(this IServiceCollection services,
        IConfigurationSection redisSection, IConfigurationSection cachingSection)
    {
        services
            .Configure<CacheOptions>(cachingSection)
            // 如果需要换成别的,这里自行切换
            .TryAddSingleton<ICacheProvider, FreeRedisCacheProvider>();
        services.TryAddSingleton<ICacheKeyGenerator, DefaultCacheKeyGenerator>();
        return services;
    }
}