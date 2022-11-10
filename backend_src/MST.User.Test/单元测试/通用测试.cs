using System.Linq.Expressions;
using MST.Infra.CacheProvider.Configuration;
using MST.Infra.CacheProvider.Interceptor;
using MST.Infra.CacheProvider.KeyGenerator;
using Xunit.Abstractions;

namespace MST.User.Test.单元测试;

public class 通用测试
{
    private readonly ITestOutputHelper _testOutputHelper;

    public 通用测试(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void 测试表达式树json序列化结果()
    {
        Expression<Func<bool>> exp = () => true;
        Expression<Func<bool>> exp2 = () => true;
        var json=exp.ToJsonString();
        var json2=exp2.ToJsonString();
        _testOutputHelper.WriteLine(json);
        Assert.Equal(json,json2);
    }

    
}