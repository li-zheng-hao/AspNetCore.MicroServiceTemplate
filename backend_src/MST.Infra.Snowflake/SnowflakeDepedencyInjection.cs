using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MST.Infra.Snowflake;

namespace AspNetCore.StartupTemplate.Snowflake;

public static class SnowflakeDepedencyInjection
{
    public static IServiceCollection AddSnowflakeGenerator(this IServiceCollection service,
       IConfiguration configuration)
    {
        service.TryAddSingleton<SnowflakeOptions>(sp => configuration.GetSection("Snowflake").Get<SnowflakeOptions>());
        service.TryAddSingleton<SnowflakeGenerator>();
        service.TryAddSingleton<SnowflakeWorkIdManager>();
        service.AddHostedService<SnowflakeBackgroundServices>();
        return service;
    }
}

public class SnowflakeOptions
{
    /// <summary>
    /// 未设置的情况下从redis中读取
    /// </summary>
    public byte? WorkId { get; set; }
    /// <summary>
    /// 默认值10 1024个节点
    /// </summary>
    public byte WorkerIdBitLength { get; set; } = 10;
    /// <summary>
    /// 默认值6 序列位长度
    /// </summary>
    public byte SeqBitLength { get; set; } = 7;
}