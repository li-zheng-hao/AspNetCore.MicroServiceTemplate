using Microsoft.Extensions.Hosting;

namespace MST.Infra.Task;
/// <summary>
/// TODO 定时任务服务 
/// </summary>
public class SchedulerBackgroundService:IHostedService
{
    private readonly SchedulerManager _schedulerManager;

    public SchedulerBackgroundService(SchedulerManager schedulerManager)
    {
        _schedulerManager = schedulerManager;
        schedulerManager.TryAddTask("test2", 5);
    }
    public System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
    {
        return System.Threading.Tasks.Task.CompletedTask;
    }

    public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
    {
        return System.Threading.Tasks.Task.CompletedTask;
    }
}