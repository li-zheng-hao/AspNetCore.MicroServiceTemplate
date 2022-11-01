using MST.Infra.Utility.Helper;
using Newtonsoft.Json;
using Rougamo.Context;

namespace MST.Infra.CacheProvider.KeyGenerator;

public class DefaultCacheKeyGenerator:ICacheKeyGenerator
{
    public string METHOD_CACHE_PREFIX { get; } = "methodcache";

    public string GeneratorKey(string customKey)
    {
        throw new NotImplementedException();
    }

    public string GeneratorKey(MethodContext methodContext)
    {
        string className = methodContext.TargetType.Name;
        string methodName = methodContext.Method.Name;
        List<object> methodArguments = methodContext.Arguments.ToList();
        string param = string.Empty;
        if (methodArguments.Count > 0)
        {
            string serializeString = JsonConvert.SerializeObject(methodArguments, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            param = ":" + EncryptHelper.Encrypt(serializeString);
        }
        return string.Concat($"{METHOD_CACHE_PREFIX}:{className}:{methodName}", param);
    }
}