using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Quickwire.Attributes;

namespace MST.Infra.Configuration;

[Description("Redis")]
[RegisterService(ServiceLifetime.Singleton)]
public class RedisOptions
{
    [InjectConfiguration("Redis:ConnectionString")]
    public string ConnectionString { get; set; }
    [InjectConfiguration("Redis:SentinelAdders")]
    public string[] SentinelAdders { get; set; }
}