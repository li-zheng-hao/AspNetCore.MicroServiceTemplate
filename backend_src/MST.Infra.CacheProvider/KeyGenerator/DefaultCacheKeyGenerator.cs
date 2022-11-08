using MST.Infra.CacheProvider.Configuration;
using MST.Infra.Utility.Helper;
using Newtonsoft.Json;
using Rougamo.Context;

namespace MST.Infra.CacheProvider.KeyGenerator;

public class DefaultCacheKeyGenerator:ICacheKeyGenerator
{
    private readonly CacheOptions _cacheOptions;

    public DefaultCacheKeyGenerator(CacheOptions cacheOptions)
    {
        _cacheOptions = cacheOptions;
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
        return string.Concat($"{methodContext.TargetType.Namespace}:{className}:{methodName}:", param);
    }
}