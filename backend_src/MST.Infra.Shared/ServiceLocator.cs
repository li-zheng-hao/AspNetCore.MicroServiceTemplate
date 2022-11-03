namespace MST.Infra.Shared;

public sealed class ServiceLocator
{
    private ServiceLocator()
    {
    }

    static ServiceLocator()
    {
    }

    /// <summary>
    /// 根级别的ServiceProvider,scope和transient级别需要创建scope,否则不会自动回收
    /// </summary>
    public static IServiceProvider? Provider { get; set; }
}