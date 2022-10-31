using Rougamo.Context;

namespace MST.Infra.CacheProvider.KeyGenerator;

public interface ICachingKeyGenerator
{
    /// <summary>
    /// 根据自定义的key生成缓存key
    /// </summary>
    /// <param name="customKey"></param>
    /// <returns></returns>
    string GeneratorKey(string customKey);

    /// <summary>
    /// 根据方法信息生成缓存Key
    /// </summary>
    /// <param name="methodContext"></param>
    /// <returns></returns>
    string GeneratorKey(MethodContext methodContext);
}