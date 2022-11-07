using FreeScheduler;
using Microsoft.Extensions.Logging;
using Quickwire.Attributes;

namespace MST.Infra.Task.Tasks;
/// <summary>
/// 示例代码 定时任务需要单独用一个进程跑,方便扩展，这里只是示例!!
/// </summary>
[RegisterService]
public class DemoTask
{
    private readonly IFreeSql _freeSql;
    private readonly ILogger<DemoTask> _logger;

    public DemoTask(IFreeSql freesql,ILogger<DemoTask> logger)
    {
        _freeSql = freesql;
        _logger = logger;
    }
    [SchedulerTask("test")]
    public void UpdateUsers(TaskInfo taskInfo)
    {
        _logger.LogInformation($"调用到了UpdateUsers方法 ");
        Thread.Sleep(5000);

    }
    [SchedulerTask("test2")]
    public void UpdateUsers2(TaskInfo taskInfo)
    {
        _logger.LogInformation($"调用到了UpdateUsers2方法 ");
        Thread.Sleep(5000);

    }
    [SchedulerTask("test3")]
    public void UpdateUsers3(TaskInfo taskInfo)
    {
        _logger.LogInformation($"调用到了UpdateUsers3方法 ");
        Thread.Sleep(5000);

    }
    [SchedulerTask("test4")]
    public void UpdateUsers4(TaskInfo taskInfo)
    {
        _logger.LogInformation($"调用到了UpdateUsers4方法 ");
        Thread.Sleep(5000);

    }
    [SchedulerTask("test5")]
    public void UpdateUsers5(TaskInfo taskInfo)
    {
        _logger.LogInformation($"调用到了UpdateUsers5方法 ");
        Thread.Sleep(5000);
    }
}