using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace 集成测试;

public class 缓存测试
{
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _application;
    private readonly HttpClient _client;

    public 缓存测试(ITestOutputHelper tempOutput)
    {
        _output = tempOutput;
        _application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // ... Configure test services
            });
        _client = _application.CreateClient();

    }
    [Fact]
    public async Task Test1()
    {
        var res=await _client.GetAsync("user/Caching/Get?num=1");
        _output.WriteLine(await res.Content.ReadAsStringAsync());
        res.EnsureSuccessStatusCode();
    }
}