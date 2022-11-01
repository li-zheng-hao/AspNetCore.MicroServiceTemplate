using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MST.Infra.FileProvider.Aliyun;

namespace MST.Infra.FileProvider;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddAliYunFileManager(this IServiceCollection collection,IConfiguration configuration)
    {
        // TODO 
        // collection.AddSingleton<IFileManagerForAliYunCOS>();
        return collection;
    } 
    public static IServiceCollection AddQCloudFileManager(this IServiceCollection collection,IConfiguration configuration)
    {
        // TODO 
        // collection.AddSingleton<IFileManagerForAliYunCOS>();
        return collection;
    } 
    public static IServiceCollection AddLocalStorageFileManager(this IServiceCollection collection,IConfiguration configuration)
    {
        // TODO 
        // collection.AddSingleton<IFileManagerForAliYunCOS>();
        return collection;
    } 
    public static IServiceCollection AddMongoFileManager(this IServiceCollection collection,IConfiguration configuration)
    {
        // TODO 
        // collection.AddSingleton<IFileManagerForAliYunCOS>();
        return collection;
    } 
}