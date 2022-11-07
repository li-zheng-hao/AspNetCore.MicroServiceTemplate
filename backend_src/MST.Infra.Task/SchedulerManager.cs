using FreeScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quickwire.Attributes;

namespace MST.Infra.Task;

/// <summary>
/// 定时任务调度管理类
/// </summary>
[RegisterService(ServiceLifetime.Singleton)]
public class SchedulerManager
{
    public SchedulerManager(IFreeSql freeSql,ITaskManager taskManager,IServiceProvider serviceProvider)
    {
        _freeSql = freeSql;
        _serviceProvider = serviceProvider;
        var tsk=_serviceProvider.GetService<CustomTaskHandler>();
        _scheduler = new Scheduler(tsk,tsk);
    }

    private static Scheduler _scheduler;
    // FreeScheduler.Scheduler scheduler;
    private readonly IFreeSql _freeSql;
    private readonly ILogger<SchedulerManager> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 新增固定间隔时间任务
    /// </summary>
    /// <param name="topic"></param>
    public (bool,string) TryAddTask(string topic,int intervalSecond=10)
    {
        var result = _freeSql.Ado.QuerySingle<dynamic>("select * from FreeScheduler_task where topic = @topic", new { topic });
        if (result is null)
        {
            var id=_scheduler.AddTask(topic, "", -1, intervalSecond);
            // var id=_scheduler.AddTaskCustom(topic, "body1", "0/1 * * * * * ");
            Console.WriteLine($"新增任务-id:{id}-topic:{topic}-间隔:{intervalSecond}");
            return (true, id);
        }
        return (false, string.Empty);
    }
    
    /// <summary>
    /// 新增根据cron表达式间隔任务
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="cronExp"></param>
    /// <returns></returns>
    public (bool,string) TryAddCustomTask(string topic,string cronExp)
    {
        var result = _freeSql.Ado.QuerySingle<dynamic>("select * from FreeScheduler_task where topic = @topic", new { topic });
        if (result is null)
        {
            var id=_scheduler.AddTaskCustom(topic, "",cronExp);
            Console.WriteLine($"新增任务-id:{id}-topic:{topic}-间隔:{cronExp}");
            return (true, id);
        }
        return (false, string.Empty);
    }

}
