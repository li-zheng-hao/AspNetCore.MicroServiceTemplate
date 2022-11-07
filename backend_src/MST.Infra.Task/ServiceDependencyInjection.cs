using Microsoft.Extensions.DependencyInjection;
using Quickwire;

namespace MST.Infra.Task;

public static class SchedulerDependencyInjection
{
    public static IServiceCollection AddScheduler(this IServiceCollection serviceCollection)
    {
        var assembly = typeof(TaskManager).Assembly;
        serviceCollection.ScanAssembly(typeof(TaskManager).Assembly, it => true);
        serviceCollection.AddHostedService<SchedulerBackgroundService>();
        return serviceCollection;
    }
}