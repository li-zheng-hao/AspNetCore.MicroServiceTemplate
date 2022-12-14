using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using MST.Infra.CacheProvider.Interceptor;
using MST.Infra.CacheProvider.Interface;
using MST.Infra.Model;
using MST.Infra.Rpc.Rest;
using MST.Infra.Shared.Contract.HttpResponse;
using Nacos.V2;
using Nacos.V2.Naming;
using Quickwire.Attributes;

namespace MST.User.Webapi.Controllers;

[ApiController]
[RegisterService]
[Route("user/[controller]/[action]")]
[InjectAllInitOnlyProperties]
public class CachingController:ControllerBase
{
    public ILogger<CachingController> _logger { get; init; }
    
    [CachingEnable]
    [HttpGet]
    public string Get(int num,[FromQuery]TestDto dto)
    {
        return $"{num}  {DateTime.Now}";
    }
    [ClearCache(typeof(CachingController),nameof(Get))]
    [HttpPost]
    public string ClearCache()
    {
        return "已清除，重新调用对应接口测试";
    }
}

public class TestDto:ICacheKey
{
    public int a { get; set; }
    public int b { get; set; }
    public int c { get; set; }
    public string ToCacheKey()
    {
        return $"{a}-{b}";
    }
}