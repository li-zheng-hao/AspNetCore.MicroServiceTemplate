using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using 集成测试.Core;

namespace 集成测试;

public class 缓存测试:IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _application;
    private readonly HttpClient _client;

    public 缓存测试(ITestOutputHelper tempOutput,CustomWebApplicationFactory<Program> factory)
    {
        _output = tempOutput;
        _application = factory;
        // _application = new WebApplicationFactory<Program>()
        //     .WithWebHostBuilder(builder =>
        //     {
        //         // ... Configure test services
        //     });
        _client = _application.CreateClient();

    }
    [Fact]
    public async Task 示例缓存接口调用()
    {
        var res=await _client.GetAsync("user/Caching/Get?num=1");
        _output.WriteLine(await res.Content.ReadAsStringAsync());
        res.EnsureSuccessStatusCode();
    }

    [Fact]
    public void 环境测试()
    {
        int i = 1;
    }
}