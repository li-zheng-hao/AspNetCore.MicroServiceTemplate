using Rougamo;

namespace MST.Infra.CacheProvider.Interceptor;
[AttributeUsage(AttributeTargets.Method,AllowMultiple = true,Inherited = false)]
public class CachingClearAttribute:MoAttribute
{
    
}