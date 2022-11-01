using FreeRedis;

namespace MST.Infra.Shared.Configuration;

public class RedisOptions
{
    public ConnectionStringBuilder ConnectionString { get; set; }
    public string[] SentinelAdders { get; set; }
}