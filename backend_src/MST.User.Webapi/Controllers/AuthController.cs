using Microsoft.AspNetCore.Mvc;
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
public class AuthController:ControllerBase
{
    public ILogger<AuthController> _logger { get; init; }
    public IAuthRestClient _authRestClient { get; init; }
    public INacosNamingService _NacosNamingService { get; init; }

    public AuthController()
    {
        
    }
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<HttpResponseResult> LoginTest(LoginRequestDto dto)
    {
        var res=await _authRestClient.LoginAsync(dto);
        return HttpResponseResult.Success(res.Content);
    }
    /// <summary>
    /// 刷新token
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<HttpResponseResult> RefreshTokenTest(LoginRequestDto dto)
    {
        var res=await _authRestClient.LoginAsync(dto);
        return HttpResponseResult.Success(res.Content);
    }
    /// <summary>
    /// 刷新token
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<HttpResponseResult> GetAllServiceInstances()
    {
        var instances1 = await _NacosNamingService.GetAllInstances("userservice","public");
        var instances2 = await _NacosNamingService.GetAllInstances("authservice","public");
        return HttpResponseResult.Success(new {instances1,instances2});
    }
}