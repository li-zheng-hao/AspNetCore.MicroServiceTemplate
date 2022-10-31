using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MST.Infra.Shared;

/// <summary>
/// 通用服务注入
/// </summary>
public static class ServiceDependencyInjection
{
    public static IServiceCollection AddRefitClient<TRestClient>(this IServiceCollection collection,List<IAsyncPolicy<HttpResponseMessage>> policies)where TRestClient : class
    {
        var refitSettings = new RefitSettings();
        collection.AddRefitClient<TRestClient>(refitSettings).SetHandlerLifetime(TimeSpan.FromMinutes(2))
        return collection;
    }
}