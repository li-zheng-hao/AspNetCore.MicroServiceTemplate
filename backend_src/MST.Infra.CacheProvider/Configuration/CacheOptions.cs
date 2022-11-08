using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.CacheProvider.Configuration
{
    /// <summary>
    /// redis options.
    /// </summary>
    [RegisterService(ServiceLifetime.Singleton)]
    public  class CacheOptions
    {
        /// <summary>
        /// 默认缓存时间
        /// </summary>
        [InjectConfiguration("Cache:RedisCacheExpireSec")]
        public double RedisCacheExpireSec { get; set; } = 300;

    }
}