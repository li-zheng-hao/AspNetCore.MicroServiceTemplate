using AspNetCore.StartupTemplate.Snowflake;
using FreeRedis;
using Microsoft.Extensions.Logging;

namespace MST.Infra.Snowflake;

public class SnowflakeWorkIdManager
{
    private readonly IRedisClient _redisClient;
    private readonly ILogger<SnowflakeWorkIdManager> _logger;
    private readonly SnowflakeOptions _options;
    private int CUR_WORK_ID { get; set; } = default!;
    public const string WORKID_COLLECTION_KEY = "snowflake_workid_set";
    public const string WORKID_CUR_INDEX = "snowflake_workid_cur_index";
    public static byte WorkId { get;  set; }
    public SnowflakeWorkIdManager(SnowflakeOptions opt,IRedisClient redisClient,ILogger<SnowflakeWorkIdManager> logger)
    {
        _options = opt;
        _logger = logger;
        _redisClient = redisClient;
        InitWorkId();
    }

    private int GetMaxWorkId()
    {
        return 1 << _options.WorkerIdBitLength;
    }
    private void InitWorkId()
    {
        if (_options.WorkId != null)
        {
            WorkId = _options.WorkId.Value;
            RefreshWorkId();
        }
        else
        {
            var curWorkId=_redisClient.Incr(WORKID_CUR_INDEX);
            if (curWorkId>GetMaxWorkId())
            {
                var workids=_redisClient.ZRangeByScore(WORKID_COLLECTION_KEY, 0, GetTimeStamp(DateTime.Now.AddSeconds(-15)), 0, 1);
                if (workids == null || workids.Length == 0)
                {
                    _logger.LogCritical("无法获取可用WorkId!");
                    throw new Exception("无法获取可用WorkId!");
                }
                WorkId = Convert.ToByte(workids[0]);
            }
            else
            {
                WorkId = (byte)curWorkId;
                RefreshWorkId();
            }
        }
       
    }
    
    public void RefreshWorkId()
    {
        _redisClient.ZAdd(WORKID_COLLECTION_KEY, (decimal)GetTimeStamp(), WorkId.ToString());
    }

    private long GetTimeStamp(DateTime? now=null)
    {
        var oldTime=new DateTime(2022,1,1);
        if (now != null)
            return now.Value.Ticks - oldTime.Ticks;
        else
            return DateTime.Now.Ticks - oldTime.Ticks;
    }
    public void UnRegisterWorkId()
    {
        // modified: 修改成后台服务中StopAsync调用
        // TODO 这里必须新建一个才能在程序退出时删除，否则会提示连接池已经释放  
        // using RedisClient redisClient = new RedisClient(
        //     GlobalConfig.Instance.Redis.RedisConn+",poolsize=1",
        //     GlobalConfig.Instance.Redis.SentinelAdders.ToArray(),
        //     true //是否读写分离
        //     
        // );
        // redisClient.ZRem(WORKID_COLLECTION_KEY, WorkId.ToString());
        _redisClient.ZRem(WORKID_COLLECTION_KEY, WorkId.ToString());

    }
   
    
    
}

