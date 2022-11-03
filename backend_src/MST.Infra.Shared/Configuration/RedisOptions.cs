using System.ComponentModel;
using FreeRedis;

namespace MST.Infra.Shared.Configuration;

[Description("Redis")]
public class RedisOptions
{
    public ConnectionStringBuilder ConnectionString { get; set; }
    public string[] SentinelAdders { get; set; }
}