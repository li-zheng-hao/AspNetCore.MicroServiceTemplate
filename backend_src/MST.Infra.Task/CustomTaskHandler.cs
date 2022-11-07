using Cronos;
using FreeScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quickwire.Attributes;

namespace MST.Infra.Task;

[RegisterService(ServiceLifetime.Singleton)]
public class CustomTaskHandler :  FreeScheduler.TaskHandlers.FreeSqlHandler,FreeScheduler.ITaskIntervalCustomHandler
{
    private readonly ITaskManager _taskManager;
    private readonly ILogger<CustomTaskHandler> _logger;
    // 最多同时有多个个任务在执行
    private int _maxParallelNum = 20;
    public CustomTaskHandler(IFreeSql fsql, ITaskManager taskManager,ILogger<CustomTaskHandler> logger) : base(fsql)
    {
        _taskManager = taskManager;
        _logger = logger;
    }
    
    public override void OnExecuting(FreeScheduler.Scheduler scheduler, TaskInfo task)
    {
        base.OnExecuting(scheduler, task);
        _logger.LogInformation($"开始执行任务 当前任务信息{ task.Id} {task.Topic}");
        _taskManager.InvokeTask(task);
    }

    public TimeSpan? NextDelay(TaskInfo task)
    {
        _logger.LogError(task.Topic+" 任务计算自定义时间");
        CronExpression expression = CronExpression.Parse("* * * * *");
        DateTime? nextUtcTime = expression.GetNextOccurrence(DateTime.UtcNow);
        var ts=nextUtcTime-DateTime.UtcNow;
        return ts;
    }
}